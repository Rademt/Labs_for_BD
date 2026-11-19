using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public interface IPaymentGateway
    {
        bool ProcessPayment(int orderId, decimal amount);
    }

    public interface INotificationService
    {
        void SendSms(string phone, string message);
        void NotifyLowStock(string productName, int remainingStock);
    }

    public class BankPaymentGateway : IPaymentGateway
    {
        private readonly AuditLogger _logger;

        public BankPaymentGateway(AuditLogger logger)
        {
            _logger = logger;
        }

        public bool ProcessPayment(int orderId, decimal amount)
        {
            if (amount <= 0) return false;
            _logger.Log("PAYMENT_GATEWAY", $"Транзакція для замовлення №{orderId} на суму {amount:C2} успішно підтверджена банком.");
            return true;
        }
    }

    public class SmsNotificationService : INotificationService
    {
        private readonly AuditLogger _logger;

        public SmsNotificationService(AuditLogger logger)
        {
            _logger = logger;
        }

        public void SendSms(string phone, string message)
        {
            _logger.Log("SMS_GATEWAY", $"Відправлено SMS на {phone}: \"{message}\"");
        }

        public void NotifyLowStock(string productName, int remainingStock)
        {
            _logger.Log("SUPPLIER_ALERT", $"[БП-3] Критичний залишок товару \"{productName}\" ({remainingStock} шт.). Сформовано заявку постачальнику.");
        }
    }
}
