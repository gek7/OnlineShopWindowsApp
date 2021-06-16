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
using MaterialDesignExtensions.Controls;
using System.Collections.ObjectModel;
using MaterialDesignExtensions.Model;
using System.Collections;
using MaterialDesignThemes.Wpf;

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

        private ItemSourceSuggestion _searchResult;
        public ItemSourceSuggestion SearchResult
        {
            get
            {
                return _searchResult;
            }
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
        }

        public static User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                if (value != null && !string.IsNullOrEmpty(value.token))
                {
                    mainWindow.accountBtn.ChangeImage();
                    mainWindow.mainFrame.Content = new CabinetPage();
                }
                else
                {
                    mainWindow.mainFrame.Content = new AuthPage();
                    mainWindow.accountBtn.ChangeImage();
                }
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
                var list = value.Distinct().ToList();
                Properties.Settings.Default.WishListIds = JsonSerializer.Serialize(list, list.GetType());
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

        private void btnEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.White;
        }

        private void btnLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Button).Foreground = Brushes.Black;
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
        //public const string BaseAddress = "http://localhost:5000";

        public static string BaseAddress
        {
            get=> Properties.Settings.Default.BaseURL;
        }
        public List<Category> CategoriesList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            controlSearch.TextInput += ControlSearch_TextInput;
            mainWindow = this;
            SearchResult = new ItemSourceSuggestion();
            DataContext = this;
            FillCategories();
        }

        private async void ControlSearch_TextInput(object sender, TextCompositionEventArgs e)
        {
            /*controlSearch.SearchTerm;
            find*/
            var collection = await RequestsHelper.GetRequest<ObservableCollection<Item>>($"{MainWindow.BaseAddress}/api/items/SearchItems?search={e.Text}");
        }

        public async void FillCategories()
        {
            var response = await RequestsHelper.GetRequest<List<Category>>($"{MainWindow.BaseAddress}/api/categories/GetTreeViewCategories", false);
            if (response != null && response.SourceResponse.IsSuccessStatusCode)
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
            MainWindow.mainWindow.mainFrame.Navigate(new WishListPage());
        }

        private void OpenFindOrderPage(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.mainFrame.Navigate(new FindOrderPage());
        }

        private async void DoSearch(object sender, SearchEventArgs args)
        {
            var collection = await RequestsHelper.GetRequest<List<Item>>($"{MainWindow.BaseAddress}/api/items/SearchItems?search={args.SearchTerm}");
            if (collection.SourceResponse.IsSuccessStatusCode)
            {
                List<Item> items = collection.Obj;
                if(items != null)
                {
                    mainFrame.Content = new ItemPage(items.First().id);
                }
            }
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void MaximizeWindow(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Maximized;

        private void CloseWindow(object sender, RoutedEventArgs e) => this.Close();

        //Метод для вызова модального окна (Фреймворк MaterialDesign).
        public static async Task<bool> ExecuteBoolDialog(UserControl u)
        {
            var view = u;
            return (bool)(await DialogHost.Show(view));
        }
    }


    //Класс для поиска
    public class ItemSourceSuggestion : ISearchSuggestionsSource
    {
        private List<string> items;

        public ItemSourceSuggestion()
        {
            FillAutoComplete();
        }

        public async void FillAutoComplete()
        {
            List<Item> result = new List<Item>();
            var collection = await RequestsHelper.GetRequest<List<Item>>($"{MainWindow.BaseAddress}/api/items/getAllItems");

            if (collection != null && collection.SourceResponse.IsSuccessStatusCode)
            {
                result = collection.Obj;
                items = result.Select(item => item.name).ToList();
            }
        }

        public IList GetAutoCompletion(string searchTerm)
        {
            if (items != null)
            {
                return items.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            return null;
        }

        public IList GetSearchSuggestions()
        {
            return null;
        }

        IList<string> ISearchSuggestionsSource.GetAutoCompletion(string searchTerm)
        {
            if (items != null)
            {
                return items.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();
            }
            return null;
        }

        IList<string> ISearchSuggestionsSource.GetSearchSuggestions()
        {
            return null;
        }
    }
}
