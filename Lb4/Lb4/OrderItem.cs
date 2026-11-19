using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public decimal Subtotal => Quantity * PriceAtPurchase;

        public override string ToString()
        {
            return $"   • [Item ID: {Id}] {ProductName,-15} x {Quantity} шт. = {Subtotal:C2} (по {PriceAtPurchase:C2}/шт)";
        }
    }
}
