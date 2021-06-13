using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class Unit
    {
        public long id { get; set; }
        public string name { get; set; }
        public string fullName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Unit)
            {
                return (obj as Unit).id == this.id;
            }
            return obj == this;
        }

        public override int GetHashCode()
        {
            return 1877310944 + id.GetHashCode();
        }
    }
}
