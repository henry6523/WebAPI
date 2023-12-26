using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using ModelsLayer.DTO;
using ModelsLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;

        public CourseRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Courses> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public Courses GetCourse(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void AddCourse(int categoryId, Courses courseCreate)
        {
            courseCreate.Categories = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

            _context.Courses.Add(courseCreate);
                _context.SaveChanges();
        }

        public void UpdateCourse(int id, CreateCourseDTO createCourseDTO)
        {
            var existingCourse = _context.Courses.FirstOrDefault(c => c.Id == id);

            if (existingCourse == null)
            {
                return;
            }

            existingCourse.CourseName = createCourseDTO.CourseName;

            _context.SaveChanges();
        }

        public void DeleteCourse(Courses courseDelete)
        {
            _context.Courses.Remove(courseDelete);
            _context.SaveChanges();
        }
    }
}
