using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;

namespace BusinessLogicLayer.DTO
{
	public class AddressDTO
	{

        [Required(ErrorMessage = "Address 1 is required")]
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip Code")]
        public string ZipCode { get; set; }
    }
}

