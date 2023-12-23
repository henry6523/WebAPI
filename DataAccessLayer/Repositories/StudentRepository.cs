using BusinessLogicLayer.DTO;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Students> GetStudents()
        {
            return _context.Students.ToList();
        }

        public Students GetStudentByCard(string studentCard)
        {
            return _context.Students.FirstOrDefault(s => s.StudentCard == studentCard);
        }

        public Students GetStudentTrimToUpper(StudentDTO studentCreate)
        {
            return GetStudents().Where(c => c.StudentName.Trim().ToUpper() == studentCreate.StudentName.TrimEnd().ToUpper())
                .FirstOrDefault();
        }

        public void AddStudent(int courseId, Students student)
        {
            var studentCourseEntity = _context.Courses.Where(a => a.Id == courseId).FirstOrDefault();

            var studentCourse = new StudentCourses()
            {
                Students = student,
                Courses = studentCourseEntity,
            };

            _context.Add(studentCourse);


            _context.Add(student);
            _context.SaveChanges();
        }

        public void UpdateStudent(Students studentUpdate)
        {
            _context.Students.Update(studentUpdate);
            _context.SaveChanges();
        }

        public void DeleteStudent(Students studentDelete)
        {
            _context.Students.Remove(studentDelete);
            _context.SaveChanges();
        }
    }
}
