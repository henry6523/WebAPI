using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class StudentCourses
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public Students Students { get; set; }
        public Courses Courses { get; set; }
    }
}
