using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public string CategoriesName { get; set; }
        public ICollection<Courses> Courses { get; set; }
    }
}
