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

        public User(long id, string login, string password, string firstName, string lastName, string image, string role, DateTime? registerDate, DateTime? lastLoginDate)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Image = image;
            Role = role;
            RegisterDate = registerDate;
            LastLoginDate = lastLoginDate;
        }

        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
    }
}
