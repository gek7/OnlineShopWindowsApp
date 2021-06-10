using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class Category : RootModel
    {
        public string name { get; set; }
        public Category owner { get; set; }
        public List<Category> categories { get; set; }
        public string image { get; set; }
        public string LoadImageString
        {
            get
            {
                if (!String.IsNullOrEmpty(image))
                {
                    return $"{MainWindow.BaseAddress}/api/categories/CategoryImage/{image}";
                }
                return "/CategoryPhoto.png";
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
