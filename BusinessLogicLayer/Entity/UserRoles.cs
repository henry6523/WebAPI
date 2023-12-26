using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.Entity
{
    public class UserRoles
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public Users Users { get; set; }
        public Roles Roles { get; set; }
    }
}
