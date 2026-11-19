using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb4
{
    public class AuditLogger
    {
        private readonly List<string> _logs = new();

        public void Log(string category, string message)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss.fff}] [{category}] {message}";
            _logs.Add(entry);
        }

        public void PrintAuditLog()
        {
            Console.WriteLine("=== ЖУРНАЛ АУДИТУ ТА ПРОДУКТИВНОСТІ (НЕФУНКЦІОНАЛЬНІ ВИМОГИ) ===");
            foreach (var entry in _logs)
            {
                Console.WriteLine(entry);
            }
            Console.WriteLine();
        }
    }
}
