using System.Collections.Generic;

namespace UniversityApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new List<Student>();

        public override string ToString() => $"Академічна група '{Name}'";
    }
}