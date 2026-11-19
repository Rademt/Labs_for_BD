using System.Text;

namespace Lb4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var logger = new AuditLogger();
            var paymentGateway = new BankPaymentGateway(logger);
            var notificationService = new SmsNotificationService(logger);

            var productManager = new ProductManager(logger, notificationService);
            var salesManager = new SalesManager(productManager, paymentGateway, notificationService, logger);

            var catBakery = productManager.AddCategory("Хлібобулочні вироби");
            var catDairy = productManager.AddCategory("Молочні продукти");
            var catDrinks = productManager.AddCategory("Напої");

            var p1 = productManager.AddProduct("Багет", 25.00m, 50, catBakery.Id);
            var p2 = productManager.AddProduct("Кефір", 42.50m, 30, catDairy.Id);
            var p3 = productManager.AddProduct("Ковбаса Салямі", 140.00m, 15, catDairy.Id);
            var p4 = productManager.AddProduct("Кокосовий Раф", 65.00m, 20, catDrinks.Id);

            productManager.PrintCatalog();

            var c1 = salesManager.RegisterCustomer("Тараканов Родіон", "0671234567");
            var c2 = salesManager.RegisterCustomer("Стус Василій", "0509876543");

            Console.WriteLine("=== ОФОРМЛЕННЯ ТА ОПЛАТА ЗАМОВЛЕНЬ ===");

            var order1 = salesManager.CreateOrder(c1.Id);
            salesManager.AddItemToOrder(order1.Id, p1.Id, 2);
            salesManager.AddItemToOrder(order1.Id, p4.Id, 1);
            salesManager.CheckoutAndPayOrder(order1.Id);
            salesManager.PrintReceipt(order1.Id);

            var order2 = salesManager.CreateOrder(c2.Id);
            salesManager.AddItemToOrder(order2.Id, p3.Id, 2);
            salesManager.AddItemToOrder(order2.Id, p2.Id, 3);
            salesManager.CheckoutAndPayOrder(order2.Id);
            salesManager.PrintReceipt(order2.Id);

            productManager.PrintCatalog();
            logger.PrintAuditLog();
        }
    }
}
