using System.Text;

namespace Lab1
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            var repository = new ShopRepository();
            var infoService = new ShopInfoService(repository);

            var bakery = repository.AddCategory("Хлібобулочні вироби");
            var dairy = repository.AddCategory("Молочні продукти");
            var drinks = repository.AddCategory("Напої");

            var rodion = repository.AddCustomer("Тараканов Родіон", "0671234567");
            var stus = repository.AddCustomer("Стус Василій", "0509876543");

            repository.AddDiscountCard(rodion.Id, "PD-32", 5, new DateTime(2026, 9, 1));
            repository.AddDiscountCard(stus.Id, "PD-32", 10, new DateTime(2026, 9, 1));

            var baguette = repository.AddProduct("Багет", 25.00m, bakery.Id);
            var kefir = repository.AddProduct("Кефір", 42.50m, dairy.Id);
            var salami = repository.AddProduct("Ковбаса Салямі", 140.00m, dairy.Id);
            var raf = repository.AddProduct("Кокосовий Раф", 65.00m, drinks.Id);

            var order1 = repository.CreateOrder(rodion.Id);
            repository.AddOrderItem(order1.Id, baguette.Id, 2);
            repository.AddOrderItem(order1.Id, raf.Id, 1);

            var order2 = repository.CreateOrder(stus.Id);
            repository.AddOrderItem(order2.Id, salami.Id, 2);
            repository.AddOrderItem(order2.Id, kefir.Id, 3);

            infoService.PrintCategoriesWithProducts();
            infoService.PrintDiscountCards();
            infoService.PrintProductsWithOrders();

            infoService.PrintCustomerFullInfo(rodion.Id);
        }
    }
}
