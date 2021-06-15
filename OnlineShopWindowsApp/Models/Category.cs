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

namespace OnlineShopWindowsApp.Models
{
    public class Category : RootModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public string name { get; set; }
        public Category owner { get; set; }
        public List<Category> categories { get; set; }
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
            try
            {
                LoadImage = (ImageSource)new ImageSourceConverter().ConvertFrom(arr);
            }
            catch
            {
                LoadImage = null;
            }
            OnPropertyChanged("LoadImage");
        }
        [JsonIgnore]
        public ImageSource LoadImage { get; set; }
        public string LoadImageString
        {
            get
            {
                if (!String.IsNullOrEmpty(image))
                {
                    return $"{MainWindow.BaseAddress}/api/categories/CategoryImage/{image}";
                }
                else
                {
                    return $"{MainWindow.BaseAddress}/api/categories/CategoryImage/unknown";
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Category)
            {
                return (obj as Category).id == this.id;
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
