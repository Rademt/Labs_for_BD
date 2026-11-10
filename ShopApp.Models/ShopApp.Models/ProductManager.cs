using System;
using System.Collections.Generic;
using System.Linq;
using ShopApp.Models;

namespace ShopApp.Managers
{
    public class ProductManager
    {
        private readonly List<Category> _categories = new();
        private readonly List<Product> _products = new();
        private int _nextCategoryId = 1;
        private int _nextProductId = 1;

        public Category AddCategory(string name)
        {
            var category = new Category { Id = _nextCategoryId++, Name = name };
            _categories.Add(category);
            return category;
        }

        public Product AddProduct(string name, decimal price, int stockQuantity, int categoryId)
        {
            var product = new Product
            {
                Id = _nextProductId++,
                Name = name,
                Price = price,
                StockQuantity = stockQuantity,
                CategoryId = categoryId
            };
            _products.Add(product);
            return product;
        }

        public bool UpdateStock(int productId, int quantityChange)
        {
            var product = GetProductById(productId);
            if (product == null) return false;

            if (product.StockQuantity + quantityChange < 0)
                return false;

            product.StockQuantity += quantityChange;
            return true;
        }

        public Product? GetProductById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public List<Product> GetAllProducts() => _products;
        public List<Category> GetAllCategories() => _categories;

        public void PrintCatalog()
        {
            Console.WriteLine("=== КАТАЛОГ ТОВАРІВ СКУЛУ/МАГАЗИНУ ===");
            foreach (var product in _products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }
    }
}