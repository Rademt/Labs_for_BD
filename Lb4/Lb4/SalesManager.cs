using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public class SalesManager
    {
        private const decimal DiscountThreshold = 300.00m;
        private const decimal DiscountRate = 0.05m;

        private readonly ProductManager _productManager;
        private readonly IPaymentGateway _paymentGateway;
        private readonly INotificationService _notificationService;
        private readonly AuditLogger _logger;

        private readonly List<Customer> _customers = new();
        private readonly List<Order> _orders = new();
        private int _nextCustomerId = 1;
        private int _nextOrderId = 101;
        private int _nextOrderItemId = 1;

        public SalesManager(
            ProductManager productManager,
            IPaymentGateway paymentGateway,
            INotificationService notificationService,
            AuditLogger logger)
        {
            _productManager = productManager;
            _paymentGateway = paymentGateway;
            _notificationService = notificationService;
            _logger = logger;
        }

        public Customer RegisterCustomer(string fullName, string phone)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Дані клієнта не можуть бути порожніми.");

            var customer = new Customer
            {
                Id = _nextCustomerId++,
                FullName = fullName,
                Phone = phone
            };
            _customers.Add(customer);
            _logger.Log("CUSTOMERS", $"Зареєстровано клієнта: {fullName} ({phone})");
            return customer;
        }

        public Order CreateOrder(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId)
                ?? throw new ArgumentException("Клієнта не знайдено!");

            var order = new Order
            {
                Id = _nextOrderId++,
                CustomerId = customerId,
                CustomerName = customer.FullName,
                OrderDate = DateTime.Now
            };

            _orders.Add(order);
            _logger.Log("ORDERS", $"Створено замовлення №{order.Id} для клієнта {customer.FullName}");
            return order;
        }

        public bool AddItemToOrder(int orderId, int productId, int quantity)
        {
            if (quantity <= 0) return false;

            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            var product = _productManager.GetProductById(productId);

            if (order == null || product == null) return false;

            if (!_productManager.UpdateStock(productId, -quantity))
            {
                Console.WriteLine($"[ПОМИЛКА БП-1] Недостатньо товару \"{product.Name}\" на складі! Доступно: {product.StockQuantity}");
                _logger.Log("VALIDATION_ERROR", $"Відмова додавання \"{product.Name}\" до замовлення №{orderId}: брак залишку.");
                return false;
            }

            var orderItem = new OrderItem
            {
                Id = _nextOrderItemId++,
                OrderId = orderId,
                ProductId = productId,
                ProductName = product.Name,
                Quantity = quantity,
                PriceAtPurchase = product.Price
            };

            order.Items.Add(orderItem);
            ApplyBusinessRulesDiscount(order);
            return true;
        }

        private void ApplyBusinessRulesDiscount(Order order)
        {
            decimal subtotal = order.CalculateSubtotal();
            if (subtotal >= DiscountThreshold)
            {
                order.DiscountAmount = Math.Round(subtotal * DiscountRate, 2);
            }
            else
            {
                order.DiscountAmount = 0;
            }
        }

        public bool CheckoutAndPayOrder(int orderId)
        {
            var stopwatch = Stopwatch.StartNew();

            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null || !order.Items.Any()) return false;

            decimal finalAmount = order.CalculateFinalTotal();

            if (_paymentGateway.ProcessPayment(order.Id, finalAmount))
            {
                order.Status = "Оплачено";
                var customer = _customers.First(c => c.Id == order.CustomerId);

                _notificationService.SendSms(
                    customer.Phone,
                    $"Ваше замовлення №{order.Id} на суму {finalAmount:C2} успішно оплачено!"
                );

                stopwatch.Stop();
                _logger.Log("PERFORMANCE", $"Замовлення №{order.Id} оброблено за {stopwatch.ElapsedMilliseconds} мс (ліміт: < 100 мс).");
                return true;
            }

            return false;
        }

        public void PrintReceipt(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            Console.WriteLine($"==========================================");
            Console.WriteLine($"ФІСКАЛЬНИЙ ЧЕК №{order.Id} від {order.OrderDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Клієнт: {order.CustomerName} [ID: {order.CustomerId}]");
            Console.WriteLine($"Статус: {order.Status}");
            Console.WriteLine($"------------------------------------------");
            foreach (var item in order.Items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"Сума без знижки:   {order.CalculateSubtotal():C2}");
            if (order.DiscountAmount > 0)
            {
                Console.WriteLine($"Знижка (БП-2, 5%): -{order.DiscountAmount:C2}");
            }
            Console.WriteLine($"РАЗОМ ДО СПЛАТИ:   {order.CalculateFinalTotal():C2}");
            Console.WriteLine($"==========================================\n");
        }
    }
}
