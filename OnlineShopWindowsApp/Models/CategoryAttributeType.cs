using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class CategoryAttributeType : RootModel
    {
        public string name { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is CategoryAttributeType)
            {
                return (obj as CategoryAttributeType).id == this.id;
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
