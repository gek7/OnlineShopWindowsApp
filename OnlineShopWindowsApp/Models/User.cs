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

        }
        public string token { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string image { get; set; }
        public string roleName { get; set; }
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
