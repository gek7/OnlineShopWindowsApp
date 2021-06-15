using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.Models
{
    public class ItemAttributeValue : INotifyPropertyChanged
    {
        private string _value;
        private Unit _unit;

        public ItemAttributeValue()
        {
        }
        public long id { get; set; }
        public long? itemAttributeId { get; set; }
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }
        public Unit unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

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
