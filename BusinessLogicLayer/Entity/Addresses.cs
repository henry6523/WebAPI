using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.Entity
{
    public class Addresses
    {
        [Key]

        public int Id { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public Students Students { get; set; }
    }
}
