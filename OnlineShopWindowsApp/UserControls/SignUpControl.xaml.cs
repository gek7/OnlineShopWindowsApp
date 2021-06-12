using OnlineShopWindowsApp.Models;
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
    /// Логика взаимодействия для SignUpControl.xaml
    /// </summary>
    public partial class SignUpControl : UserControl
    {
        public User user { get; set; }
        public Action backClick { get; set; }
        public SignUpControl()
        {
            InitializeComponent();
            user = new User();
            DataContext = this;
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            backClick?.Invoke();
        }

        private async void DoSignUp(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(user.login))
            {
                HelperClass.message("Логин должен быть заполнен");
                return;
            }
            if (string.IsNullOrWhiteSpace(PwdRegBox.Password))
            {
                HelperClass.message("Пароль должен быть заполнен");
                return;
            }
            if(PwdRegBox.Password != PwdAgainBox.Password)
            {
                HelperClass.message("Пароли должны совпадать");
                return;
            }
            if (string.IsNullOrEmpty(user.firstName) || string.IsNullOrEmpty(user.lastName))
            {
                HelperClass.message("Должны быть быть заполнены имя и фамилия");
                return;
            }
            user.password = PwdAgainBox.Password;
            User u = await SignUp();
            if (u != null)
            {
                MainWindow.User = u;
            }
        }
        private async Task<User> SignUp()
        {
            AdvanceResponse<User> Resp = await RequestsHelper.PostRequest<User>(MainWindow.BaseAddress + $"/api/account/SignUp", user, false); ;

            if (Resp.SourceResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Resp.Obj;
            }
            else
            {
                MessageBox.Show(await Resp.SourceResponse.Content.ReadAsStringAsync());
            }
            return null;
        }
    }
}
