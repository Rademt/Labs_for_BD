using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb5
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public decimal Subtotal => Quantity * PriceAtPurchase;
    }
}
