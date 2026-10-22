using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab1
{
    public class ShopRepository
    {
        private readonly List<Category> _categories = new();
        private readonly List<Product> _products = new();
        private readonly List<Customer> _customers = new();
        private readonly List<DiscountCard> _cards = new();
        private readonly List<Order> _orders = new();
        private readonly List<OrderItem> _orderItems = new();

        private int _nextCategoryId = 1;
        private int _nextProductId = 1;
        private int _nextCustomerId = 1;
        private int _nextCardId = 1;
        private int _nextOrderId = 101;

        public Category AddCategory(string name)
        {
            var category = new Category { Id = _nextCategoryId++, Name = name };
            _categories.Add(category);
            return category;
        }

        public Product AddProduct(string name, decimal price, int categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.Id == categoryId);
            var product = new Product
            {
                Id = _nextProductId++,
                Name = name,
                Price = price,
                CategoryId = categoryId,
                Category = category
            };

            _products.Add(product);
            category?.Products.Add(product);
            return product;
        }

        public Customer AddCustomer(string fullName, string phone)
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

        public DiscountCard AddDiscountCard(int customerId, string cardNumber, int discountPercent, DateTime issueDate)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            var card = new DiscountCard
            {
                Id = _nextCardId++,
                CustomerId = customerId,
                CardNumber = cardNumber,
                DiscountPercent = discountPercent,
                IssueDate = issueDate
            };

            _cards.Add(card);
            if (customer != null) customer.Card = card;
            return card;
        }

        public Order CreateOrder(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            var order = new Order
            {
                Id = _nextOrderId++,
                CustomerId = customerId,
                Customer = customer!,
                OrderDate = DateTime.Now
            };

            _orders.Add(order);
            customer?.Orders.Add(order);
            return order;
        }

        public void AddOrderItem(int orderId, int productId, int quantity)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            var product = _products.FirstOrDefault(p => p.Id == productId);

            if (order == null || product == null) return;

            var item = new OrderItem
            {
                OrderId = orderId,
                Order = order,
                ProductId = productId,
                Product = product,
                Quantity = quantity,
                UnitPrice = product.Price
            };

            _orderItems.Add(item);
            order.Items.Add(item);
            product.OrderItems.Add(item);
        }

        public List<Category> GetCategories() => _categories;
        public List<Customer> GetCustomers() => _customers;
        public List<Product> GetProducts() => _products;
        public List<Order> GetOrders() => _orders;

        public Customer? GetCustomerById(int id) => _customers.FirstOrDefault(c => c.Id == id);
    }
}