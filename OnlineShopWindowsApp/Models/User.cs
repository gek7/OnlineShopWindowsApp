using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OnlineShopWindowsApp.Models
{
    public class User : RootModel, INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public User()
        {
            role = new Role();

        }
        public string token { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        private string _img;
        public string image
        {
            get
            {
                return _img;
            }
            set
            {
                _img = value;
                CalculateLoadImage();

            }
        }
        public async void CalculateLoadImage()
        {
            byte[] arr = await RequestsHelper.getByteArray(LoadImageString);
            LoadImage = (ImageSource)new ImageSourceConverter().ConvertFrom(arr);
            OnPropertyChanged("LoadImage");
        }
        public Role role { get; set; }
        public string LoadImageString
        {
            get
            {
                if (!String.IsNullOrEmpty(image))
                {
                    return $"{MainWindow.BaseAddress}/api/account/ProfileImage/{image}";
                }
                else
                {
                    return $"{MainWindow.BaseAddress}/api/account/ProfileImage/unknown";
                }
            }
        }
        [JsonIgnore]
        public ImageSource LoadImage { get; set; }
        public Nullable<System.DateTime> registerDate { get; set; }
    }
}