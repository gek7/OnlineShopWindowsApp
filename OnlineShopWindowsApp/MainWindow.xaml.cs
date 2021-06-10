using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Text.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OnlineShopWindowsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static User _user; 
        /*{token= "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjEwMDA2Iiwicm9sZSI6IkFkbWluaXN0cmF0b3IiLCJuYmYiOjE2MjI4MDg4NDAsImV4cCI6MTYyNTQwMDg0MCwiaWF0IjoxNjIyODA4ODQwfQ.aDy-yCitPcbDucmZgKXMuxtimLqT0T2kMKilKNdekwA",
                                                id = 10006, 
                                                role = new Role() {id=1, name="Administrator" } };*/
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public List<long> WishListIds
        {
            get
            {
                string src = Properties.Settings.Default.WishListIds;
                if (string.IsNullOrEmpty(src)) src = "[]";
                return (List<long>)JsonSerializer.Deserialize(src, typeof(List<long>));
            }
            set
            {
                List<long> oldIds  = WishListIds;
                oldIds.AddRange(value);
                Properties.Settings.Default.WishListIds = JsonSerializer.Serialize(oldIds.Distinct().ToList(), typeof(List<long>));
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public List<long> CartIds
        {
            get
            {
                string src = Properties.Settings.Default.CartIds;
                if (string.IsNullOrEmpty(src)) src = "[]";
                return (List<long>)JsonSerializer.Deserialize(src, typeof(List<long>));
            }
            set
            {
                Properties.Settings.Default.CartIds = JsonSerializer.Serialize(value, typeof(List<long>));
                Properties.Settings.Default.Save();
                OnPropertyChanged();
            }
        }

        public void AddItemToCart(long id)
        {
            var listIds = CartIds;
            listIds.Add(id);
            CartIds = listIds;
        }

        public void AddItemToWishList(long id)
        {
            var listIds = WishListIds;
            listIds.Add(id);
            WishListIds = listIds;
        }

        public void SubItemFromCart(long id)
        {
            var listIds = CartIds;
            if (listIds.Contains(id))
            {
                listIds.Remove(id);
            }
            CartIds = listIds;
        }

        public void SubItemFromWishList(long id)
        {
            var listIds = WishListIds;
            if (listIds.Contains(id))
            {
                listIds.Remove(id);
            }
            WishListIds = listIds;
        }

        public static MainWindow mainWindow { get; set; }
        public const string BaseAddress = "http://localhost:5000";
        public List<Category> CategoriesList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
            DataContext = this;
            FillCategories();
        }

        public async void FillCategories()
        {
            var response = await RequestsHelper.GetRequest<List<Category>>($"{MainWindow.BaseAddress}/api/categories/GetTreeViewCategories", false);
            if (response.SourceResponse.IsSuccessStatusCode)
            {
                CategoriesControl.Categories = response.Obj;
            }
        }

        private void testBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void ClickCart(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.mainFrame.Navigate(new CartPage());
        }

        private void ClickWishList(object sender, RoutedEventArgs e)
        {

        }
    }
}
