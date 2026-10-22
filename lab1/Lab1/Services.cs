using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class ShopInfoService
    {
        private readonly ShopRepository _repository;

        public ShopInfoService(ShopRepository repository)
        {
            _repository = repository;
        }

        public void PrintCategoriesWithProducts()
        {
            Console.WriteLine("СПИСОК КАТЕГОРІЙ ТА ТОВАРІВ");
            Console.WriteLine("------------------------------------------");
            foreach (var category in _repository.GetCategories())
            {
                Console.WriteLine($"[+] {category.Name}");
                foreach (var product in category.Products)
                {
                    Console.WriteLine($"    - {product.Name} (Ціна: {product.Price:C2}) [ID: {product.Id}]");
                }
            }
            Console.WriteLine("------------------------------------------\n");
        }

        public void PrintDiscountCards()
        {
            Console.WriteLine("РЕЄСТР ДИСКОНТНИХ КАРТОК");
            Console.WriteLine("------------------------------------------");
            foreach (var customer in _repository.GetCustomers())
            {
                if (customer.Card != null)
                {
                    Console.WriteLine($"{customer.FullName} => {customer.Card}");
                }
            }
            Console.WriteLine("------------------------------------------\n");
        }

        public void PrintProductsWithOrders()
        {
            Console.WriteLine("ТОВАРИ ТА ЗАМОВЛЕННЯ (ЗВ'ЯЗОК БАГАТО-ДО-БАГАТЬОХ)");
            Console.WriteLine("------------------------------------------");
            foreach (var product in _repository.GetProducts())
            {
                Console.WriteLine($"Товар: {product.Name} ({product.Price:C2})");
                if (!product.OrderItems.Any())
                {
                    Console.WriteLine("  - Поки немає у замовленнях");
                    continue;
                }

                foreach (var item in product.OrderItems)
                {
                    Console.WriteLine($"  - Входить до Замовлення №{item.OrderId} (Клієнт: {item.Order.Customer.FullName}) — {item.Quantity} шт.");
                }
            }
            Console.WriteLine("------------------------------------------\n");
        }

        public void PrintCustomerFullInfo(int customerId)
        {
            var customer = _repository.GetCustomerById(customerId);
            if (customer == null) return;

            Console.WriteLine("==========================================");
            Console.WriteLine($"КАРТКА КЛІЄНТА: {customer.FullName.ToUpper()}");
            Console.WriteLine($"Телефон: {customer.Phone}");
            Console.WriteLine($"Картка знижки: {(customer.Card != null ? customer.Card.ToString() : "Немає")}");
            Console.WriteLine("Історія замовлень:");

            if (!customer.Orders.Any())
            {
                Console.WriteLine(" * Немає оформлених замовлень");
            }
            else
            {
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine($" * Замовлення №{order.Id} від {order.OrderDate:yyyy-MM-dd}:");
                    decimal total = 0;
                    foreach (var item in order.Items)
                    {
                        Console.WriteLine($"    • {item.Product.Name} x {item.Quantity} шт. = {item.Subtotal:C2}");
                        total += item.Subtotal;
                    }
                    Console.WriteLine($"   Сума замовлення: {total:C2}");
                }
            }
            Console.WriteLine("==========================================\n");
        }
    }
}
