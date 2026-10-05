using System;
using System.Collections.Generic;
using System.Text;

namespace Lb2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Инициализация категорий
            List<Category> categories = new List<Category>
            {
                new Category(1, "Хлібобулочні вироби"),
                new Category(2, "Молочні продукти"),
                new Category(3, "Напої")
            };

            // 2. Инициализация товаров
            List<Product> products = new List<Product>
            {
                new Product(1, "Багет", 25.00m, 50, 1),
                new Product(2, "Кефір", 42.50m, 30, 2),
                new Product(3, "Ковбаса Салямі", 140.00m, 15, 2),
                new Product(4, "Кокосовий Раф", 65.00m, 20, 3)
            };

            // 3. Инициализация клиентов
            List<Customer> customers = new List<Customer>
            {
                new Customer(1, "Тараканов Родіон", "0671234567"),
                new Customer(2, "Стус Василій", "0509876543")
            };

            // Вывод категорий
            Console.WriteLine("=== КАТЕГОРІЇ ТОВАРІВ ===");
            foreach (var category in categories)
            {
                Console.WriteLine(category);
            }
            Console.WriteLine();

            // Вывод клиентов
            Console.WriteLine("=== БАЗА КЛІЄНТІВ ===");
            foreach (var customer in customers)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine();

            // Вывод каталога товаров
            Console.WriteLine("=== КАТАЛОГ ТОВАРІВ МАГАЗИНУ ===");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();

            // 4. Оформление заказов
            Console.WriteLine("=== ОФОРМЛЕНІ ЗАМОВЛЕННЯ ===");
            Order order1 = new Order(101, customers[0]);
            order1.AddItem(products[0], 2); // 2 багеты
            order1.AddItem(products[3], 1); // 1 раф
            order1.PrintReceipt();

            Order order2 = new Order(102, customers[1]);
            order2.AddItem(products[2], 2); // 2 салями
            order2.AddItem(products[1], 3); // 3 кефира
            order2.PrintReceipt();

            // 5. Вывод обновленного остатка
            Console.WriteLine("=== ОНОВЛЕНИЙ ЗАЛИШОК НА СКЛАДІ ===");
            foreach (var product in products)
            {
                Console.WriteLine(product);
            }
        }
    }
}