using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class ItemAttributeValue
    {
        public ItemAttributeValue()
        {
        }
        public long id { get; set; }
        public long? itemAttributeId { get; set; }
        public string value { get; set; }
        public Unit unit { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ItemAttributeValue)
            {
                return (obj as ItemAttributeValue).id == this.id;
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
