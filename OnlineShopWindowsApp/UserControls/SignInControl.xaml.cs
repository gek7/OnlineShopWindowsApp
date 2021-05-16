using OnlineShopWindowsApp.Pages;
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
        public Action CallBack { get; set; }
        public SignInControl()
        {
            InitializeComponent();
        }
        private void RegClick(object sender, RoutedEventArgs e)
        {
            regClick?.Invoke();
        }
        private async void loginClick(object sender, RoutedEventArgs e)
        {

            //Проверка
            HttpClient client = new HttpClient();
            string token = await GetToken(LoginTextBox.Text, PwdBox.Password);
            if (token != null)
            {
                MainWindow.UserToken = token;
            }
            else
            {
                MainWindow.mainWindow.MainSnackbar
                    .MessageQueue?.Enqueue("Неверный логин или пароль");
            }
            CallBack?.Invoke();
        }

        private async Task<string> GetToken(string login, string pwd)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync($"http://localhost:45065/api/account/auth?username={login}&password={pwd}", new StringContent(""));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
