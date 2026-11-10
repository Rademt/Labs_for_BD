using System;
using System.Collections.Generic;
using System.Linq;
using ShopApp.Models;

namespace ShopApp.Managers
{
    public class SalesManager
    {
        private readonly ProductManager _productManager;
        private readonly List<Customer> _customers = new();
        private readonly List<Order> _orders = new();
        private int _nextCustomerId = 1;
        private int _nextOrderId = 101;
        private int _nextOrderItemId = 1;

        public SalesManager(ProductManager productManager)
        {
            _productManager = productManager;
        }

        public Customer RegisterCustomer(string fullName, string phone)
        {
            var customer = new Customer
            {
                Id = _nextCustomerId++,
                FullName = fullName,
                Phone = phone
            };
            _customers.Add(customer);
            return customer;
        }

        public Order CreateOrder(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new ArgumentException("Клієнта не знайдено!");

            var order = new Order
            {
                Id = _nextOrderId++,
                CustomerId = customerId,
                CustomerName = customer.FullName,
                OrderDate = DateTime.Now
            };

            _orders.Add(order);
            return order;
        }

        public bool AddItemToOrder(int orderId, int productId, int quantity)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            var product = _productManager.GetProductById(productId);

            if (order == null || product == null) return false;

            if (product.StockQuantity < quantity)
            {
                Console.WriteLine($"[ПОМИЛКА] Недостатньо товару \"{product.Name}\" на складі! Доступно: {product.StockQuantity}");
                return false;
            }

            _productManager.UpdateStock(productId, -quantity);

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
            return true;
        }

        public void PrintReceipt(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            Console.WriteLine($"==========================================");
            Console.WriteLine($"ЧЕК ЗАМОВЛЕННЯ №{order.Id} від {order.OrderDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Клієнт: {order.CustomerName} [ID Клієнта: {order.CustomerId}]");
            Console.WriteLine($"Статус: {order.Status}");
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine("Товари:");
            foreach (var item in order.Items)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"------------------------------------------");
            Console.WriteLine($"РАЗОМ ДО СПЛАТИ: {order.CalculateTotal():C2}");
            Console.WriteLine($"==========================================\n");
        }

        public List<Customer> GetAllCustomers() => _customers;
        public List<Order> GetAllOrders() => _orders;
    }
}