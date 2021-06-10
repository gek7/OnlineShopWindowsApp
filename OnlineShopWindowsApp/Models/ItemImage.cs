using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class ItemImage
    {
        public ItemImage()
        {
        }
        public long? id { get; set; }
        public string path { get; set; }
        public bool isMain { get; set; }
    }
}
