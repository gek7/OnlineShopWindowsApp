using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class CategoryAttributeModel : RootModel
    {
        public string name { get; set; }
        public CategoryAttributeType attrType { get; set; }
        public long? category { get; set; }
    }
}
