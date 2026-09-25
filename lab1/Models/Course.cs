using System.Collections.Generic;

namespace UniversityApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Credits { get; set; }

        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public override string ToString() => $"Дисципліна: {Title} ({Credits} cr.)";
    }
}