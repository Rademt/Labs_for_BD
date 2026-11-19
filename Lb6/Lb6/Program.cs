using System.Text;

namespace Lb6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var customerRepo = new CustomerRepository();
            var inventoryService = new InventoryService();
            var paymentService = new PaymentService();
            var orderController = new OrderController(customerRepo, inventoryService, paymentService);

            Console.WriteLine("=== 1. Основний потік виконання (Успішне оформлення замовлень) ===");
            orderController.ProcessCustomerOrder("Тараканов Родіон", "0671234567", "Багет", 2, 50.00m);
            orderController.ProcessCustomerOrder("Стус Василій", "0509876543", "Ковбаса Салямі", 2, 280.00m);
            orderController.ProcessCustomerOrder("Ковальчук Олена", "0635554433", "Кокосовий Раф", 1, 65.00m);

            Console.WriteLine("\n=== 2. Альтернативний потік (фрагмент alt на діаграмі) ===");
            orderController.ProcessCustomerOrder("Тараканов Родіон", "0671234567", "Кефір", 2, 85.00m);

            orderController.ProcessCustomerOrder("Бондаренко Ігор", "0971112233", "Ковбаса Салямі", 20, 2800.00m);

            orderController.ProcessCustomerOrder("Петренко Олексій", "0952223344", "Кокосовий Раф", 2, 50.00m);

            Console.WriteLine("\n=== ПІДСУМКОВИЙ РЕЄСТР ОФОРМЛЕНИХ ЗАМОВЛЕНЬ ===");
            foreach (var order in paymentService.GetAllOrders())
            {
                Console.WriteLine($"Замовлення №{order.Id,-3} | Клієнт: {order.Customer.FullName,-18} | Товар: {order.Item.ProductName,-15} (Item ID: {order.Item.Id}) | К-сть: {order.Item.Quantity} шт. | Сплачено: {order.TotalPaid:C2}");
            }
        }
    }
}
