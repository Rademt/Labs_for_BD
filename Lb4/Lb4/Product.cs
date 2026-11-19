using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    namespace ShopApp.Models
    {
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public int CategoryId { get; set; }

            public override string ToString()
            {
                return $"ID: {Id,-2} | {Name,-18} | Ціна: {Price,8:C2} | Залишок: {StockQuantity} шт.";
            }
        }
    }
}
