using System.Text;

namespace Lb5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var catalog = new Dictionary<int, Product>
            {
                { 1, new Product { Id = 1, Name = "Багет", Price = 25.00m, StockQuantity = 50 } },
                { 2, new Product { Id = 2, Name = "Кефір", Price = 42.50m, StockQuantity = 30 } },
                { 3, new Product { Id = 3, Name = "Ковбаса Салямі", Price = 140.00m, StockQuantity = 15 } },
                { 4, new Product { Id = 4, Name = "Кокосовий Раф", Price = 65.00m, StockQuantity = 20 } }
            };

            var stateMachine = new OrderStateMachine(catalog);

            Console.WriteLine("=== СЦЕНАРІЙ 1: Успішне оформлення та доставка замовлення ===");
            var order1 = new OrderContext
            {
                OrderId = 101,
                CustomerName = "Тараканов Родіон",
                DeliveryAddress = "м. Київ, вул. Солом'янська, 7",
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 1, ProductId = 1, ProductName = "Багет", Quantity = 2, PriceAtPurchase = 25.00m },
                    new OrderItem { Id = 2, ProductId = 4, ProductName = "Кокосовий Раф", Quantity = 1, PriceAtPurchase = 65.00m }
                }
            };
            stateMachine.PayOrder(order1, 115.00m);
            stateMachine.ShipOrder(order1);
            stateMachine.ConfirmDelivery(order1);

            Console.WriteLine("\n=== СЦЕНАРІЙ 2: Оплата, відправка та повернення товару (<= 14 днів) ===");
            var order2 = new OrderContext
            {
                OrderId = 102,
                CustomerName = "Стус Василій",
                DeliveryAddress = "м. Біла Церква, бул. Олександрійський, 45",
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 3, ProductId = 3, ProductName = "Ковбаса Салямі", Quantity = 2, PriceAtPurchase = 140.00m }
                }
            };
            stateMachine.PayOrder(order2, 280.00m);
            stateMachine.ShipOrder(order2);
            stateMachine.ReturnOrder(order2, daysSinceDelivery: 5);

            Console.WriteLine("\n=== СЦЕНАРІЙ 3: Блокування переходу (недостатня сума оплати) ===");
            var order3 = new OrderContext
            {
                OrderId = 103,
                CustomerName = "Ковальчук Олена",
                DeliveryAddress = "м. Київ, пр-т Берестейський, 12",
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 4, ProductId = 2, ProductName = "Кефір", Quantity = 3, PriceAtPurchase = 42.50m }
                }
            };
            stateMachine.PayOrder(order3, 100.00m);

            Console.WriteLine("\n=== СЦЕНАРІЙ 4: Скасування замовлення та блокування дій у кінцевому стані ===");
            var order4 = new OrderContext
            {
                OrderId = 104,
                CustomerName = "Бондаренко Ігор",
                DeliveryAddress = "м. Київ, вул. Хрещатик, 22",
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = 5, ProductId = 1, ProductName = "Багет", Quantity = 1, PriceAtPurchase = 25.00m }
                }
            };
            stateMachine.PayOrder(order4, 25.00m);
            stateMachine.CancelOrder(order4);
            stateMachine.ShipOrder(order4);

            Console.WriteLine("\n=== ПІДСУМКОВИЙ СТАН ОБ'ЄКТІВ (КОНТЕКСТІВ) ===");
            var allOrders = new[] { order1, order2, order3, order4 };
            foreach (var o in allOrders)
            {
                Console.WriteLine($"Замовлення №{o.OrderId,-3} | Клієнт: {o.CustomerName,-18} | Сума: {o.TotalAmount,7:C2} | Стан: {o.CurrentState}");
            }
        }
    }
}
