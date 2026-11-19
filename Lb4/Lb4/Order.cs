using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal DiscountAmount { get; set; }
        public string Status { get; set; } = "Нове";

        public decimal CalculateSubtotal() => Items.Sum(item => item.Subtotal);
        public decimal CalculateFinalTotal() => CalculateSubtotal() - DiscountAmount;
    }
}
