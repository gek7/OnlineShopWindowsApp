using OnlineShopWindowsApp.Pages;
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
    /// Логика взаимодействия для AccountButton.xaml
    /// </summary>
    public partial class AccountButton : UserControl
    {
        public AccountButton()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenPage();
        }

        public void OpenPage()
        {
            if (HelperClass.isAuth())
            {
                MainWindow.mainWindow.mainFrame.Content = new CabinetPage();
            }
            else
            {
                MainWindow.mainWindow.mainFrame.Content = new AuthPage();
            }
        }

        public void SetImage(BitmapImage img)
        {
            if (img !=null)
            {
                icon.Visibility = Visibility.Collapsed;
                UserImg.Source = img;
            }
        }
            
        public void ChangeImage()
        {
            if (MainWindow.User != null)
            {

                icon.Visibility = Visibility.Visible;
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Account;

            }
            else
            {
                icon.Visibility = Visibility.Visible;
                icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Login;
            }
        }
    }
}
