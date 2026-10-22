using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class DiscountCard
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public int DiscountPercent { get; set; }
        public DateTime IssueDate { get; set; }

        public override string ToString()
        {
            return $"Дисконтна картка №{CardNumber} (Знижка: {DiscountPercent}%, видано: {IssueDate:yyyy-MM-dd})";
        }
    }
}
