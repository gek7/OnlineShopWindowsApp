using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class Review : RootModel
    {
        public string reviewContent { get; set; }
        public long item { get; set; }
        public User user { get; set; }
        public int itemMark { get; set; }
    }
}
