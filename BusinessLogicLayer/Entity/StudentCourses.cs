using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLayer.Entity
{
    public class StudentCourses
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public Students Students { get; set; }
        public Courses Courses { get; set; }
    }
}
