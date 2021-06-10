using OnlineShopWindowsApp.Pages.AdministratorSubPages;
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

namespace OnlineShopWindowsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdministratorPage.xaml
    /// </summary>
    public partial class AdministratorPage : Page
    {
        public AdministratorPage()
        {
            InitializeComponent();
        }

        private void OpenUserFrame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UsersPage());
        }

        private void OpenCategoryFrame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdministratorSubPages.CategoriesPage());
        }

        private void OpenItemsFrame(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdministratorSubPages.ItemsPage());
        }
    }
}
