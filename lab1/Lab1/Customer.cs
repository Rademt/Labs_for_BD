using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public DiscountCard? Card { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();

        public override string ToString() => $"{FullName} (Тел: {Phone}) [ID: {Id}]";
    }
}
