using HandyControl.Controls;
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
        private double _minimum;
        private double _maximum;

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

        public double Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                _minimum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Minimum"));
            }
        }

        public double Maximum
        {
            get
            {
                return _maximum;
            }
            set
            {
                _maximum = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Maximum"));
            }
        }

        //(([0 - 9]{1},[0-9]{1}))| ([0 - 9] +)
        public RangeControl(decimal? minumum, decimal? maximum)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Minimum = minumum == null ? 0 :(double) minumum;
            this.Maximum = maximum == null ? 0 :(double) maximum;
            if (Minimum == Maximum) Maximum++;
            Slider.ValueStart = Minimum;
            Slider.ValueEnd = Maximum;
        }

        public RangeControl()  : this (0, 1000) { }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
