using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Entity
{
    public class Teachers
    {
        [Key]
        public int Id { get; set; }
        public string TeacherName { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
        public ICollection<TeachersCourses> TeachersCourses { get; set; }

    }
}
