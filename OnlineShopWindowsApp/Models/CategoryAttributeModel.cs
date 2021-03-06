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

        public override bool Equals(object obj)
        {
            if (obj is CategoryAttributeModel)
            {
                return (obj as CategoryAttributeModel).id == this.id;
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
