using System;
using UniversityApp.Repository;

namespace UniversityApp.Services
{
    public class UniversityInfoService
    {
        private readonly UniversityRepository _repo;

        public UniversityInfoService(UniversityRepository repo)
        {
            _repo = repo;
        }

        public void PrintGroupsWithStudents()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("СПИСОК ГРУП ТА СТРУКТУРА СТУДЕНТІВ");
            Console.WriteLine("----------------------------------");

            foreach (var grp in _repo.GetGroups())
            {
                Console.WriteLine($"[+] {grp.Name}");
                if (grp.Students.Count == 0)
                {
                    Console.WriteLine("    (немає студентів)");
                    continue;
                }

                foreach (var st in grp.Students)
                {
                    Console.WriteLine($"    • {st.FullName} (ID: {st.Id})");
                }
            }
            Console.WriteLine();
        }

        public void PrintStudentCards()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("РЕЄСТР СТУДЕНТСЬКИХ КВИТКІВ");
            Console.WriteLine("----------------------------------");

            foreach (var st in _repo.GetStudents())
            {
                string cardStatus = st.Card != null ? st.Card.ToString() : "Квиток ще не оформлено";
                Console.WriteLine($"{st.FullName} => {cardStatus}");
            }
            Console.WriteLine();
        }

        public void PrintCoursesWithStudents()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("ДИСЦИПЛІНИ ТА ЗАПИСАНІ СТУДЕНТИ");
            Console.WriteLine("----------------------------------");

            foreach (var crs in _repo.GetCourses())
            {
                Console.WriteLine($"Предмет: {crs.Title} ({crs.Credits} кр.)");
                foreach (var enr in crs.Enrollments)
                {
                    string score = enr.Grade.HasValue ? $"{enr.Grade} балів" : "без оцінки";
                    Console.WriteLine($"  - {enr.Student.FullName}: {score}");
                }
            }
            Console.WriteLine();
        }

        public void PrintStudentFullInfo(int studentId)
        {
            var st = _repo.GetStudentById(studentId);
            if (st == null)
            {
                Console.WriteLine($"[!] Студента з ідентифікатором {studentId} не знайдено.");
                return;
            }

            Console.WriteLine("==================================================");
            Console.WriteLine($"КАРТКА СТУДЕНТА: {st.FullName.ToUpper()}");
            Console.WriteLine($"Група: {st.Group?.Name ?? "не призначена"}");
            Console.WriteLine($"Документ: {(st.Card != null ? st.Card.CardNumber : "відсутній")}");
            Console.WriteLine("Успішність з дисциплін:");

            foreach (var enr in st.Enrollments)
            {
                Console.WriteLine($"  * {enr.Course.Title} — {enr.Grade?.ToString() ?? "N/A"}");
            }
            Console.WriteLine("==================================================");
            Console.WriteLine();
        }
    }
}