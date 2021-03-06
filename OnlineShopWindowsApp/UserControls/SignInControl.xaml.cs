using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для SignInControl.xaml
    /// </summary>
    public partial class SignInControl : UserControl
    {
        public Action regClick { get; set; }
        public SignInControl()
        {
            InitializeComponent();
        }
        private void RegClick(object sender, RoutedEventArgs e)
        {
            regClick?.Invoke();
        }
        private async void login(object sender, RoutedEventArgs e)
        {
            //Проверка
            HttpClient client = new HttpClient();
            User user = await GetUser(LoginTextBox.Text, PwdBox.Password);
            if (user != null)
            {
                MainWindow.User = user;
            }
        }

        private async Task<User> GetUser(string login, string pwd)
        {
            AuthData data = new AuthData();
            data.username = login;
            data.password = pwd;
            AdvanceResponse<User> Resp = await RequestsHelper.PostRequest<User, AuthData>(MainWindow.BaseAddress + $"/api/account/auth", data, false); ;

            if(Resp.SourceResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Resp.Obj;
            }
            return null;
        }
    }
}
