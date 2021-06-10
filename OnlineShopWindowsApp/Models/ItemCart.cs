using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class ItemCart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ItemCart(Item itm)
        {
            id = itm.id;
            name = itm.name;
            priceForOne = itm.price;
            LoadImageString = itm.LoadImageString;
            images = itm.images;
        }

        public long id { get; set; }
        public string name { get; set; }
        public decimal? priceForOne { get; set; }
        public List<ItemImage> images { get; set; }

        private long _count;
        public long Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                OnPropertyChanged();
                OnPropertyChanged("Price");
            }
        }

        public double Price
        {
            get
            {
                if (priceForOne == null) return 0;
                return ((double)priceForOne) * Count;
            }
        }

        public string LoadImageString { get; set; }

        public static List<ItemCart> ConvertToItemCart(List<Item> items, bool isCart = true, bool isOwnCount = false)
        {
            List<long> ids = MainWindow.mainWindow.CartIds;
            var distinctItems = items.Distinct().ToList();
            List<ItemCart> itms = new List<ItemCart>();
            distinctItems.ForEach(i =>
            {
                ItemCart ic = new ItemCart(i);
                if (isOwnCount)
                {
                    ic.Count = items.Where(item => item.id == i.id).Count();
                }
                else if (isCart)
                {
                    ic.Count = ids.Where(itm => itm == i.id).Count();
                }
                itms.Add(ic);
            });
            return itms;
        }
    }
}
