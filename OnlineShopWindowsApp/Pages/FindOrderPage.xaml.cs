using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using OnlineShopWindowsApp.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Логика взаимодействия для FindOrderPage.xaml
    /// </summary>
    public partial class FindOrderPage : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private Order _order;
        public Visibility isVisibility { get; set; }
        public Order order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                if(value != null)
                {
                    isVisibility = Visibility.Visible;
                }
                else
                {
                    isVisibility = Visibility.Collapsed;
                }

                OnPropertyChanged();
                OnPropertyChanged("isVisibility");
            }
        }
        public FindOrderPage()
        {
            InitializeComponent();
            isVisibility = Visibility.Collapsed;
            DataContext = this;
        }

        private async void FindOrder(object sender, RoutedEventArgs e)
        {
            var response = await RequestsHelper.GetRequest<Order>($"{MainWindow.BaseAddress}/api/orders/getOrderByNum?num={orderNumTb.Text}");
           if (response.SourceResponse.IsSuccessStatusCode)
            {
                itemPanel.Children.Clear();
                if (response.Obj != null)
                {
                    order = response.Obj;
                    if (response.Obj.items.Count > 0)
                    {
                        List<ItemCart> items = ItemCart.ConvertToItemCart(response.Obj.items, false, true);
                        items.ForEach(i =>
                        {
                            ItemCartControl icc = new ItemCartControl(false, true) ;
                            icc.DataContext = i;
                            itemPanel.Children.Add(icc);
                        });
                    }
                }
                else
                {
                    itemPanel.Children.Add(new TextBlock() { Text="Заказ не найден", FontSize = 40, HorizontalAlignment=HorizontalAlignment.Center});
                }
            }
        }
    }
}
