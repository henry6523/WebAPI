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
		
        public string Address_1 { get; set; }

		public string Address_2 { get; set; }

		public string City { get; set; }

		public string ZipCode { get; set; }
	}
}

