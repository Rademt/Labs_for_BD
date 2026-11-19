using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb6
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers = new();
        private int _nextId = 1;

        public Customer SaveOrGetCustomer(string fullName, string phone)
        {
            Console.WriteLine($"  [OrderController -> CustomerRepository]: SaveOrGetCustomer(\"{fullName}\", \"{phone}\")");

            var existing = _customers.FirstOrDefault(c => c.Phone == phone);
            if (existing != null)
            {
                Console.WriteLine($"  [CustomerRepository --> OrderController]: повернуто існуючого Customer (Id={existing.Id})");
                return existing;
            }

            var customer = new Customer
            {
                Id = _nextId++,
                FullName = fullName,
                Phone = phone
            };
            _customers.Add(customer);

            Console.WriteLine($"  [CustomerRepository --> OrderController]: повернуто нового Customer (Id={customer.Id})");
            return customer;
        }
    }

    public class InventoryService
    {
        private readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Багет", Price = 25.00m, StockQuantity = 50 },
            new Product { Id = 2, Name = "Кефір", Price = 42.50m, StockQuantity = 30 },
            new Product { Id = 3, Name = "Ковбаса Салямі", Price = 140.00m, StockQuantity = 10 },
            new Product { Id = 4, Name = "Кокосовий Раф", Price = 65.00m, StockQuantity = 20 }
        };

        private int _nextOrderItemId = 1;

        public OrderItem? ReserveProduct(string productName, int quantity)
        {
            Console.WriteLine($"  [OrderController -> InventoryService]: ReserveProduct(\"{productName}\", {quantity})");

            var product = _products.FirstOrDefault(p => p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
            if (product == null || product.StockQuantity < quantity)
            {
                Console.WriteLine($"  [InventoryService --> OrderController]: return null (недостатньо товару на складі)");
                return null;
            }

            product.StockQuantity -= quantity;
            var item = new OrderItem
            {
                Id = _nextOrderItemId++,
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            Console.WriteLine($"  [InventoryService --> OrderController]: повернуто OrderItem (Id={item.Id}, Сума={item.Subtotal:C2})");
            return item;
        }

        public void RollbackReservation(OrderItem item)
        {
            var product = _products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.StockQuantity += item.Quantity;
            }
        }
    }

    public class PaymentService
    {
        private readonly List<OrderRecord> _orders = new();
        private int _nextOrderId = 101;

        public OrderRecord? ConfirmAndPayOrder(Customer customer, OrderItem? item, decimal paymentAmount)
        {
            Console.WriteLine($"  [OrderController -> PaymentService]: ConfirmAndPayOrder(CustomerId={customer.Id}, Payment={paymentAmount:C2})");

            if (item == null || paymentAmount < item.Subtotal)
            {
                Console.WriteLine($"  [PaymentService --> OrderController]: return null (Відмова: брак товару або коштів)");
                return null;
            }

            var record = new OrderRecord
            {
                Id = _nextOrderId++,
                Customer = customer,
                Item = item,
                TotalPaid = item.Subtotal
            };
            _orders.Add(record);

            Console.WriteLine($"  [PaymentService --> OrderController]: Оплату підтверджено (OrderRecord №{record.Id})");
            return record;
        }

        public List<OrderRecord> GetAllOrders() => _orders;
    }
}
