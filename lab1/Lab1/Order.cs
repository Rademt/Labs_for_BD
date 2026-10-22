using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public override string ToString() => $"Замовлення №{Id} від {OrderDate:yyyy-MM-dd HH:mm}";
    }
}
