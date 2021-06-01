using OnlineShopWindowsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OnlineShopWindowsApp
{
    public class User
    {
        public User()
        {
            role = new Role();
        }
        public string token { get; set; }
        public long id  { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string image { get; set; }
        public Role role { get; set; }
        public string LoadImageString
        { 
            get 
            {
                if (!String.IsNullOrEmpty(image)){
                    return $"{MainWindow.BaseAddress}/api/account/ProfileImage/{image}";
                }
                return "/accountImg.png";
            } 
        }
        public Nullable<System.DateTime> registerDate { get; set; }
    }
}