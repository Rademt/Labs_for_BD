using System;
using UniversityApp.Repository;
using UniversityApp.Services;

namespace UniversityApp
{
    class Program
    {
        static void Main()
        {
            var repository = new UniversityRepository();
            var infoService = new UniversityInfoService(repository);

            var kn21 = repository.AddGroup("КН-21");
            var kn22 = repository.AddGroup("КН-22");

            var artem = repository.AddStudent("Пономаренко Артем", kn21.Id);
            var angelina = repository.AddStudent("Кобилиньска Ангеліна", kn21.Id);
            var rodion = repository.AddStudent("Тараканов Родіон", kn22.Id);

            repository.AddStudentCard(artem.Id, "ST-1001", new DateTime(2024, 9, 1));
            repository.AddStudentCard(angelina.Id, "ST-1002", new DateTime(2024, 9, 1));
            repository.AddStudentCard(rodion.Id, "ST-1003", new DateTime(2024, 9, 1));

            var programming = repository.AddCourse("Програмування", 5);
            var databases = repository.AddCourse("Бази даних", 4);
            var math = repository.AddCourse("Вища математика", 3);

            repository.EnrollStudent(artem.Id, programming.Id, 95);
            repository.EnrollStudent(artem.Id, databases.Id, 88);
            repository.EnrollStudent(angelina.Id, programming.Id, 91);
            repository.EnrollStudent(angelina.Id, math.Id, 84);
            repository.EnrollStudent(rodion.Id, databases.Id, 77);
            repository.EnrollStudent(rodion.Id, math.Id, 90);

            infoService.PrintGroupsWithStudents();
            infoService.PrintStudentCards();
            infoService.PrintCoursesWithStudents();

            infoService.PrintStudentFullInfo(artem.Id);
        }
    }
}