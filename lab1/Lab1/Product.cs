using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public override string ToString() => $"Товар: {Name} ({Price:C2}) [ID: {Id}]";
    }
}
