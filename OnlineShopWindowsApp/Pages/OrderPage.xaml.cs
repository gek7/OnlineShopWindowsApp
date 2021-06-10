using OnlineShopWindowsApp.Models;
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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page, INotifyPropertyChanged
    {
        private Order _order;
        public Order Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private int _stepIndex;
        public int StepIndex
        {
            get
            {
                return _stepIndex;
            }
            set
            {
                if(value >= 0 && value <= 2)
                {
                    _stepIndex = value;
                    switch (_stepIndex)
                    {
                        case 0:
                            ContentBorder.Child = new OrderFirstStep() { DataContext = this };
                            backButton.Visibility = Visibility.Collapsed;
                            nextButton.Visibility = Visibility.Visible;
                            break;
                        case 1:
                            ContentBorder.Child = new OrderSecondStep(this);
                            nextButton.Visibility = Visibility.Collapsed;
                            break;
                    }
                    OnPropertyChanged();
                }
            }
        }
        public OrderPage()
        {
            InitializeComponent();
            StepIndex = 0;
            DataContext = this;
            Order = new Order() { orderNum="Неизвестный номер заказа"};
        }

        public OrderPage(List<ItemCart> items) : this()
        {
            items.ForEach(item =>
            {
                for (int i = 0; i < item.Count; i++)
                {
                    Order.items.Add(new Item(item));
                }
            });
        }

        private void NextPage(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(Order.phoneNumber) && !String.IsNullOrWhiteSpace(Order.deliveryAddress))
            {
                StepIndex++;
            }
            else
            {
                MessageBox.Show("Номер телефона и адрес доставки\nдолжны быть заполнены");
            }
        }

        private void BackPage(object sender, RoutedEventArgs e)
        {
            StepIndex--;
        }
    }
}
