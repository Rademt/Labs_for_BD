using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb6
{
    public class OrderController
    {
        private readonly CustomerRepository _customerRepo;
        private readonly InventoryService _inventoryService;
        private readonly PaymentService _paymentService;

        public OrderController(
            CustomerRepository customerRepo,
            InventoryService inventoryService,
            PaymentService paymentService)
        {
            _customerRepo = customerRepo;
            _inventoryService = inventoryService;
            _paymentService = paymentService;
        }

        public void ProcessCustomerOrder(string customerName, string phone, string productName, int quantity, decimal paymentAmount)
        {
            Console.WriteLine($"\n[Менеджер з продажу -> OrderController]: ProcessCustomerOrder(\"{customerName}\", \"{productName}\", {quantity} шт., {paymentAmount:C2})");

            var customer = _customerRepo.SaveOrGetCustomer(customerName, phone);

            var orderItem = _inventoryService.ReserveProduct(productName, quantity);

            var orderRecord = _paymentService.ConfirmAndPayOrder(customer, orderItem, paymentAmount);

            if (orderRecord != null)
            {
                Console.WriteLine($"[OrderController --> Менеджер з продажу]: Успіх! Замовлення №{orderRecord.Id} для {customer.FullName} оформлено (Позиція ID: {orderRecord.Item.Id}).");
            }
            else
            {
                if (orderItem != null)
                {
                    _inventoryService.RollbackReservation(orderItem);
                }
                Console.WriteLine($"[OrderController --> Менеджер з продажу]: Операцію відхилено.");
            }
        }
    }
}
