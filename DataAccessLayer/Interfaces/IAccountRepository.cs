using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        void Login(UserInfo model);
        void Register(UserDTO model);

        IEnumerable<string> GetUserRoles(string username);
    }
}
