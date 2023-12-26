using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLayer.Entity
{
    public class Classes
    {
        [Key]
        public int Id { get; set; }
        public string ClassName { get; set; }
        public ICollection<Students> Students { get; set; }

    }
}
