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

namespace OnlineShopWindowsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page, INotifyPropertyChanged
    {
        //Для scrollviewer во 2-й строке
        public double Row1Height { get; set; }
        public CartPage()
        { 
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Row1Height = this.ActualHeight-100;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Row1Height"));
        }
    }
}
