using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows;
using OnlineShopWindowsApp.ServerActions;
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

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page, IPage<Order>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public bool myOrders { get; set; }
        public OrdersPage(bool ShowMyOrders)
        {
            InitializeComponent();
            myOrders = ShowMyOrders;
            this.DataContext = this;
            RefreshGrid();
        }

        private List<Order> _dataSource;
        public List<Order> DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                OnPropertyChanged();
            }
        }

        public async void RefreshGrid()
        {
            AdvanceResponse<List<Order>> orders;
            if (!myOrders)
            {
                orders = await RequestsHelper.GetRequest<List<Order>>($"{MainWindow.BaseAddress}/api/orders/getOrders", true);
                userColumn.Visibility = Visibility.Visible;
                editBtn.Visibility = Visibility.Visible;
            }
            else
            {
                orders = await RequestsHelper.GetRequest<List<Order>>($"{MainWindow.BaseAddress}/api/orders/getMyOrders", true);
                userColumn.Visibility = Visibility.Collapsed;
                editBtn.Visibility = Visibility.Collapsed;
            }
            if (orders.SourceResponse.IsSuccessStatusCode)
            {
                DataSource = orders.Obj;
                if(orders.Obj == null || orders.Obj.Count == 0)
                {
                    dataGrid.Visibility = Visibility.Collapsed;
                    TbNoOrders.Visibility = Visibility.Visible;
                }
                else
                {
                    dataGrid.Visibility = Visibility.Visible;
                    TbNoOrders.Visibility = Visibility.Collapsed;
                }
            }
            this.DataContext = this;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                new OrderDialog(((Order)dataGrid.SelectedItem).id).ShowDialog();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }
    }
}
