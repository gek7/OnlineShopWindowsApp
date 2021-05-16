using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EnumControl.xaml
    /// </summary>
    public partial class EnumControl : UserControl, INotifyPropertyChanged
    {
        private string _header;
        public EnumControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Header"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
