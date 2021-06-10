using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для OrderSecondStep.xaml
    /// </summary>
    public partial class OrderSecondStep : UserControl
    {
        private OrderPage orderPage;

        public OrderSecondStep()
        {
            InitializeComponent();
        }

        public OrderSecondStep(OrderPage orderPage) : this()
        {
            this.orderPage = orderPage;
            CreateOrder();
        }

        public async void CreateOrder()
        {
           var response = await  RequestsHelper.PostRequest<Order>($"{MainWindow.BaseAddress}/api/orders/createOrder", orderPage.Order, true);
            orderPage.Order.orderNum = response.Obj.orderNum;
            orderPage.OnPropertyChanged("Order");

        }
    }
}
