using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp
{
    public class User
    {

        public User(string token)
        {
            Token = token;
        }
        public User(string token, string firstName, string lastName, string image, string role, DateTime? registerDate)
        {
            Token = token;
            FirstName = firstName;
            LastName = lastName;
            Image = image;
            Role = role;
            RegisterDate = registerDate;
        }

        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
    }
}
