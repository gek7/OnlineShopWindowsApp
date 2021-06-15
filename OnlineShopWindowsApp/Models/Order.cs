using OnlineShopWindowsApp.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class Order : RootModel, INotifyPropertyChanged
    {
        public Order()
        {
            items = new List<Item>();
        }
        public string orderNum { get; set; }
        public string phoneNumber { get; set; }
        public string deliveryAddress { get; set; }
        public  OrderStatus orderStatus { get; set; }
        public User user { get; set; }
        private List<Item> _items;
        public List<Item> items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("itemCarts"));
            }
        }
        [JsonIgnore]
        public List<ItemCartControl> itemCarts
        {
            get
            {
                List<ItemCartControl> controls = new List<ItemCartControl>();
                List<ItemCart> itemCarts = ItemCart.ConvertToItemCart(items, false, true);
                itemCarts.ForEach(i =>
                {
                    ItemCartControl icc = new ItemCartControl(false, true);
                    icc.DataContext = i;
                    controls.Add(icc);
                });
                return controls;
            }
        }

        public decimal? FullPrice
        {
            get
            {
                return items.Sum(i => i.price);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
