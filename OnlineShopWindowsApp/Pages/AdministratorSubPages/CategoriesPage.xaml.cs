using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages
{
    /// <summary>
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page, IPage<Category>
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public CategoriesPage()
        {
            InitializeComponent();
            this.DataContext = this;
            MainWindow.mainWindow.mainFrame.Navigating += Navigating;
            RefreshGrid();
        }

        private void Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (MainWindow.mainWindow.additionalFrame.Visibility == Visibility.Visible && 
                MainWindow.mainWindow.additionalFrame.Content is CategoryAttributesPage)
            {
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Collapsed;
            }
        }

        private List<Category> _dataSource;
        public List<Category> DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get
            {
                return dataGrid.SelectedItem as Category;
            }
        }

        private void CategoryChanged(object sender, SelectionChangedEventArgs e)
        {
            OnPropertyChanged("SelectedCategory");
        }

        public async void RefreshGrid()
        {
            var task = RequestsHelper.GetRequest<List<Category>>($"{MainWindow.BaseAddress}/api/categories/getAllCategories", true);
            await task.ContinueWith((previous) =>
            {
                List<Category> response = previous.Result.Obj.ToList();
                DataSource = response.ToList();
                this.DataContext = this;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new CategoryDialog(ActionType.Add).ShowDialog();
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var response = await RequestsHelper.DeleteRequest<Category>($"{MainWindow.BaseAddress}/api/categories?id={((Category)dataGrid.SelectedItem).id}", true);
                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    RefreshGrid();
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                new CategoryDialog(ActionType.Edit, ((Category)dataGrid.SelectedItem).id).ShowDialog();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }

        private void ShowCategoryAttribute(object sender, RoutedEventArgs e)
        {
            if(MainWindow.mainWindow.additionalFrame.Visibility == Visibility.Collapsed)
            {
                MainWindow.mainWindow.additionalFrame.Content = new CategoryAttributesPage(this);
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Collapsed;
            }
        }
    }
}
