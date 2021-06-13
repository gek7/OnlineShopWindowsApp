using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class ItemAttribute
    {
        public ItemAttribute()
        {
        }
        public long id { get; set; }
        public long? itemId { get; set; }
        public CategoryAttributeModel categoryAttribute { get; set; }
        public List<ItemAttributeValue> itemAttributesValues { get; set; }
    }
}
