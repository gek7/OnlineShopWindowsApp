using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class AuthData
    {
        public AuthData()
        {
        }

        public AuthData(string token, User authUser)
        {
            this.token = token;
            this.authUser = authUser;
        }

        public string token { get; set; }
        public User authUser { get; set; }
    }
}
