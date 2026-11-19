using Lb4.ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public class ProductManager
    {
        private const int LowStockThreshold = 15;
        private readonly List<Category> _categories = new();
        private readonly List<Product> _products = new();
        private readonly AuditLogger _logger;
        private readonly INotificationService _notificationService;

        private int _nextCategoryId = 1;
        private int _nextProductId = 1;

        public ProductManager(AuditLogger logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        public Category AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Назва категорії не може бути порожньою.");

            var category = new Category { Id = _nextCategoryId++, Name = name };
            _categories.Add(category);
            _logger.Log("CATALOG", $"Додано категорію: {name}");
            return category;
        }

        public Product AddProduct(string name, decimal price, int stockQuantity, int categoryId)
        {
            if (price <= 0 || stockQuantity < 0)
                throw new ArgumentException("Некоректна ціна або кількість товару.");

            var product = new Product
            {
                Id = _nextProductId++,
                Name = name,
                Price = price,
                StockQuantity = stockQuantity,
                CategoryId = categoryId
            };
            _products.Add(product);
            _logger.Log("CATALOG", $"Додано товар \"{name}\" (Ціна: {price:C2}, Залишок: {stockQuantity} шт.)");
            return product;
        }

        public bool UpdateStock(int productId, int quantityChange)
        {
            var product = GetProductById(productId);
            if (product == null || product.StockQuantity + quantityChange < 0)
                return false;

            product.StockQuantity += quantityChange;

            if (product.StockQuantity < LowStockThreshold)
            {
                _notificationService.NotifyLowStock(product.Name, product.StockQuantity);
            }

            return true;
        }

        public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public List<Product> GetAllProducts() => _products;

        public void PrintCatalog()
        {
            Console.WriteLine("=== КАТАЛОГ ТОВАРІВ МАГАЗИНУ ===");
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }
    }
}
