using System;
using System.Collections.Generic;
using System.Linq;
using UniversityApp.Models;

namespace UniversityApp.Repository
{
    public class UniversityRepository
    {
        private readonly List<Group> _groupList = new();
        private readonly List<Student> _studentList = new();
        private readonly List<StudentCard> _cardList = new();
        private readonly List<Course> _courseList = new();
        private readonly List<Enrollment> _enrollmentList = new();

        private int _nextGroupId = 1;
        private int _nextStudentId = 1;
        private int _nextCardId = 1;
        private int _nextCourseId = 1;

        public Group AddGroup(string groupName)
        {
            var newGroup = new Group { Id = _nextGroupId++, Name = groupName };
            _groupList.Add(newGroup);
            return newGroup;
        }

        public IEnumerable<Group> GetGroups() => _groupList.AsReadOnly();

        public Student AddStudent(string name, int targetGroupId)
        {
            var targetGroup = _groupList.Find(g => g.Id == targetGroupId)
                ?? throw new ArgumentException($"Групу з ID {targetGroupId} не знайдено.");

            var newStudent = new Student
            {
                Id = _nextStudentId++,
                FullName = name,
                GroupId = targetGroupId,
                Group = targetGroup
            };

            _studentList.Add(newStudent);
            targetGroup.Students.Add(newStudent);
            return newStudent;
        }

        public IEnumerable<Student> GetStudents() => _studentList.AsReadOnly();

        public Student? GetStudentById(int id) => _studentList.FirstOrDefault(s => s.Id == id);

        public StudentCard AddStudentCard(int studentId, string code, DateTime date)
        {
            var student = GetStudentById(studentId)
                ?? throw new ArgumentException($"Студента з ID {studentId} не існує.");

            if (student.Card != null)
                throw new InvalidOperationException("Студент вже має документ.");

            var newCard = new StudentCard
            {
                Id = _nextCardId++,
                StudentId = studentId,
                CardNumber = code,
                IssueDate = date
            };

            _cardList.Add(newCard);
            student.Card = newCard;
            return newCard;
        }

        public Course AddCourse(string courseTitle, int creditAmount)
        {
            var newCourse = new Course { Id = _nextCourseId++, Title = courseTitle, Credits = creditAmount };
            _courseList.Add(newCourse);
            return newCourse;
        }

        public IEnumerable<Course> GetCourses() => _courseList.AsReadOnly();

        public Enrollment EnrollStudent(int studentId, int courseId, int? mark = null)
        {
            var st = GetStudentById(studentId) ?? throw new ArgumentException("Студента не знайдено");
            var crs = _courseList.FirstOrDefault(c => c.Id == courseId) ?? throw new ArgumentException("Курс не знайдено");

            var record = new Enrollment
            {
                StudentId = studentId,
                Student = st,
                CourseId = courseId,
                Course = crs,
                Grade = mark
            };

            _enrollmentList.Add(record);
            st.Enrollments.Add(record);
            crs.Enrollments.Add(record);
            return record;
        }
    }
}