using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp
{
    public class User
    {
        public User()
        {

        }
        public string token { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string image { get; set; }
        public string roleName { get; set; }
        public Nullable<System.DateTime> registerDate { get; set; }
    }
}
