using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Classes
    {
        [Key]
        public int Id { get; set; }
        public string ClassName { get; set; }
        public ICollection<Students> Students { get; set; }

    }
}
