using Microsoft.Win32;
using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    /// Логика взаимодействия для CabinetPage.xaml
    /// </summary>
    public partial class CabinetPage : Page
    {
        public CabinetPage()
        {
            InitializeComponent();
            if(MainWindow.User == null || string.IsNullOrEmpty(MainWindow.User.token))
            {
                MainWindow.mainWindow.mainFrame.GoBack();
                return;
            }
            FillInfo();
            MainWindow.mainWindow.accountBtn.ChangeImage();
        }

        public async void FillInfo()
        {
            AdvanceResponse<User> resp = await RequestsHelper.GetRequest<User>(MainWindow.BaseAddress+"/api/account/info", true);
            if (resp.Obj != null)
            {
                User u = resp.Obj;
                TextblockName.Text = u.firstName + " " + u.lastName;
                if (u.image != null)
                {
                    BitmapImage newImage = new BitmapImage();
                    newImage.BeginInit();
                    newImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    newImage.CacheOption = BitmapCacheOption.OnLoad;
                    newImage.UriSource = new Uri($"{MainWindow.BaseAddress}/api/account/ProfileImage/{u.image}");
                    newImage.EndInit();
                    Img.ImageSource = newImage;
                    MainWindow.mainWindow.accountBtn.SetImage(newImage);
                }
                if (u.roleName == "Administrator")
                {
                    AdminButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                NavigationService.Navigate(new ErrorPage(resp.SourceResponse.ReasonPhrase));
            }
        }

        private async void ChangeImage(object sender, RoutedEventArgs e)
        {
             OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
               await RequestsHelper.SendFile(MainWindow.BaseAddress + "/api/account/setImage", ofd.FileName, true);
            }
            FillInfo();
        }

        private void OpenAdministratorPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdministratorPage());
        }
    }
}
