using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RangeControl.xaml
    /// </summary>
    public partial class RangeControl : UserControl, INotifyPropertyChanged
    {
        private string _header;
        private string _suffix;
        private string _prefix;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string Suffix
        {
            get
            {
                return _suffix;
            }
            set
            {
                _suffix = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Suffix"));
            }
        }

        public string Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Prefix"));
            }
        }
        //(([0 - 9]{1},[0-9]{1}))| ([0 - 9] +)
        public RangeControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
