using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitrsLite.Business.Services
{
    public class UserService
    {
        public string shortenUserName(string userName)
        {
            if (userName.StartsWith(@"DOACS\", StringComparison.OrdinalIgnoreCase))
            {
                userName = userName.Trim();
                userName = userName.Replace(@"DOACS\", "");
                userName = userName.Replace(" ", "");
                userName = userName.ToLower();
            }
            
            return userName;
        }
    }
}
