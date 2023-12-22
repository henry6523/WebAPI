using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class TeachersCourses
    {
        public int TeacherId { get; set; }
        public int CourseId { get; set; }
        public Teachers Teachers { get; set; }
        public Courses Courses { get; set; }

    }
}
