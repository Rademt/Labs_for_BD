using System;

namespace UniversityApp.Models
{
    public class StudentCard
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }

        public override string ToString()
        {
            return $"Квиток №{CardNumber} (видано: {IssueDate:yyyy-MM-dd})";
        }
    }
}