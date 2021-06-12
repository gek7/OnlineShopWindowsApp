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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;

namespace OnlineShopWindowsApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ItemControl.xaml
    /// </summary>
    public partial class ItemControl : UserControl
    {
        public ItemControl()
        {
            InitializeComponent();
        }

        private void CardMouseEnter(object sender, MouseEventArgs e)
        {
            CardItem.SetValue(MaterialDesignThemes.Wpf.ShadowAssist.ShadowDepthProperty, MaterialDesignThemes.Wpf.ShadowDepth.Depth4);
        }

        private void CardMouseLeave(object sender, MouseEventArgs e)
        {
            CardItem.SetValue(MaterialDesignThemes.Wpf.ShadowAssist.ShadowDepthProperty, MaterialDesignThemes.Wpf.ShadowDepth.Depth2);
        }

        private void AddToWishList(object sender, RoutedEventArgs e)
        {
            Item i = (sender as Button).DataContext as Item;
            MainWindow.mainWindow.AddItemToWishList(i.id);
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            Item i = (sender as Button).DataContext as Item;
            MainWindow.mainWindow.AddItemToCart(i.id);
        }

        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            long id = (DataContext as Item).id;
            MainWindow.mainWindow.mainFrame.Navigate(new ItemPage(id));
        }
    }
}
