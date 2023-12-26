using ModelsLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        void Login(UserInfos model);
        void Register(UsersDTO model);

        IEnumerable<string> GetUserRoles(string username);
        UserTokens BuildToken(UserInfos model, IEnumerable<string> userRoles);
    }
}
