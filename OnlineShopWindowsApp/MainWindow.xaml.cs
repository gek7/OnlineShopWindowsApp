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

namespace OnlineShopWindowsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static User _user;
        public static User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                mainWindow.accountBtn.ChangeImage();
                mainWindow.mainFrame.Content = new CabinetPage();
            }
        }
        public static MainWindow mainWindow { get; set; }
        public const string BaseAddress = "http://localhost:5000";
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
        }
    }
}
