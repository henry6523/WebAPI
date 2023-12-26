using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.Entity
{
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserRoles> UserRoles { get; set; }
    }
}
