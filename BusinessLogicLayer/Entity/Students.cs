using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLayer.Entity
{
    public class Students
    {
        [Key]
        public int Id { get; set; }
        public string StudentCard { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        public int PhoneNo { get; set; }
        public Addresses Address { get; set; }
        public Classes Classes { get; set; }
        public ICollection<StudentCourses> StudentCourses { get; set; }
    }
}
