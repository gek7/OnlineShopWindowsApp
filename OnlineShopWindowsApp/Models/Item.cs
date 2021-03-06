using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OnlineShopWindowsApp.Models
{
    public class Item : RootModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal? price { get; set; }
        public Category category { get; set; }
        public int? count { get; set; }
        public Item owner { get; set; }
        public List<ItemImage> images { get; set; }
        public List<Item> items { get; set; }
        public double avgRating { get; set; }

        public Item()
        {

        }
        public Item(ItemCart ic)
        {
            id = ic.id;
            name = ic.name;
            price = ic.priceForOne;
            images = ic.images;

        }

        public string LoadImageString
        {
            get
            {
                if (images != null && images.Count > 0)
                {
                    var mains = images.Where(i => i.isMain == true);
                    string image = mains.Count() > 0 ? images.Where(i => i.isMain).FirstOrDefault().path : images.FirstOrDefault().path;
                    
                    return image;
                }
                return "/CategoryPhoto.png";
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is Item)
            {
                return (obj as Item).id == this.id;
            }
            return obj == this;
        }

        //Используется для быстрого сравнения в разных методах, например в методе distinct у List<Item>
        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
