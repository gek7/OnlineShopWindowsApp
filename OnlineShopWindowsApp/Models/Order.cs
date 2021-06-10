using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class Order : RootModel
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
        public List<Item> items { get; set; }

        public decimal? FullPrice
        {
            get
            {
                return items.Sum(i => i.price);
            }
        }
    }
}
