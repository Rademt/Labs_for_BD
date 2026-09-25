using System.Collections.Generic;

namespace UniversityApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public StudentCard? Card { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public override string ToString() => $"{FullName} [ID: {Id}]";
    }
}