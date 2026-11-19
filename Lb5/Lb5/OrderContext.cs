using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb5
{
    public class OrderContext
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
        public OrderState CurrentState { get; set; } = OrderState.Created;

        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public bool IsAddressValid => !string.IsNullOrWhiteSpace(DeliveryAddress);
        public bool IsFinalState => CurrentState is OrderState.Completed or OrderState.Cancelled or OrderState.Returned;
    }
}
