using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Entity
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public Categories Categories { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
        public ICollection<TeachersCourses> TeachersCourses { get; set; }

    }
}
