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
    /// Логика взаимодействия для WishListPage.xaml
    /// </summary>
    public partial class WishListPage : Page, INotifyPropertyChanged
    {
        public WishListPage()
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
            var result = await RequestsHelper.PostRequest<List<Item>, List<long>>($"{MainWindow.BaseAddress}/api/Items/getItemsByIds", MainWindow.mainWindow.WishListIds, false);
            if (result.SourceResponse.IsSuccessStatusCode && result.Obj != null)
            {
                List<ItemCart> items = ItemCart.ConvertToItemCart(result.Obj);
                panelForItems.Children.Clear();
                items.ForEach(i =>
                {
                    ItemCartControl icc = new ItemCartControl(true, false);
                    icc.DataContext = i;
                    panelForItems.Children.Add(icc);
                });
            }
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.WishListIds = new List<long>();
            Fill();
        }
    }
}
