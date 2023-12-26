using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer.DTO
{
    public class UserTokens
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
