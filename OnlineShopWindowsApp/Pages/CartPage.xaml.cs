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
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page, INotifyPropertyChanged
    {
        public double SummaryPrice { get; set; }

        public CartPage()
        { 
            InitializeComponent();
            this.DataContext = this;
            Fill();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public async void Fill()
        {
           var result = await RequestsHelper.PostRequest<List<Item>, List<long>>($"{MainWindow.BaseAddress}/api/Items/getItemsByIds", MainWindow.mainWindow.CartIds, false);
            if(result.SourceResponse.IsSuccessStatusCode && result.Obj != null)
            {
                List<ItemCart> items = ItemCart.ConvertToItemCart(result.Obj);
                items.ForEach(i =>
                {
                    ItemCartControl icc = new ItemCartControl();
                    icc.SourceChanged += Icc_SourceChanged;
                    icc.DataContext = i;
                    ItemsContainer.Children.Add(icc);
                });
                Icc_SourceChanged(this, null);
            }
        }

        private void Icc_SourceChanged(object sender, EventArgs e)
        {
            double sum = 0;
            foreach (var child in ItemsContainer.Children)
            {
                sum += ((child as ItemCartControl).DataContext as ItemCart).Price;
            }
            SummaryPrice = sum;
            OnPropertyChanged("SummaryPrice");
        }

        private void ClearCart(object sender, RoutedEventArgs e)
        {
            while(ItemsContainer.Children.Count > 0)
            {
                (ItemsContainer.Children[0] as ItemCartControl).DeleteItem();
            }
        }

        private void OpenOrderPage(object sender, RoutedEventArgs e)
        {
            if(ItemsContainer.Children.Count > 0)
            {
                List<ItemCart> ics = new List<ItemCart>();
                foreach (var child in ItemsContainer.Children)
                {
                    ics.Add(((child as ItemCartControl).DataContext as ItemCart));
                }
                MainWindow.mainWindow.mainFrame.Navigate(new OrderPage(ics));
            }
            else
            {
                HelperClass.message("Нет товаров в корзине");
            }
        }
    }
}
