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
    /// Логика взаимодействия для ItemsPage.xaml
    /// </summary>
    public partial class ItemsPage : Page, INotifyPropertyChanged, IPage<Item>
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ItemsPage()
        {
            InitializeComponent();
            this.DataContext = this;
            MainWindow.mainWindow.mainFrame.Navigating += Navigating;
            RefreshGrid();
        }

        private void Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (MainWindow.mainWindow.additionalFrame.Visibility == Visibility.Visible &&
                MainWindow.mainWindow.additionalFrame.Content is ItemAttrubutesPage)
            {
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Collapsed;
            }
        }

        private List<Item> _dataSource;
        public List<Item> DataSource
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

        public async void RefreshGrid()
        {
            var task = RequestsHelper.GetRequest<List<Item>>($"{MainWindow.BaseAddress}/api/Items/getAllItems", true);
            await task.ContinueWith((previous) =>
            {
                List<Item> response = previous.Result.Obj.ToList();
                DataSource = response.ToList();
                this.DataContext = this;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new ItemDialog(ActionType.Add).ShowDialog();
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                var response = await RequestsHelper.DeleteRequest<Item>($"{MainWindow.BaseAddress}/api/Items?id={((Item)dataGrid.SelectedItem).id}", true);
                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    RefreshGrid();
                }
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                new ItemDialog(ActionType.Edit, ((Item)dataGrid.SelectedItem).id).ShowDialog();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }

        private void RichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            RichTextBox rt = (sender as RichTextBox);
            Item i = rt.DataContext as Item;
            HelperClass.SetXaml(rt, i.description);
        }

        private void ShowItemAttribute(object sender, RoutedEventArgs e)
        {
            if (MainWindow.mainWindow.additionalFrame.Visibility == Visibility.Collapsed)
            {
                MainWindow.mainWindow.additionalFrame.Content = new ItemAttrubutesPage(this);
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Visible;
            }
            else
            {
                MainWindow.mainWindow.additionalFrame.Visibility = Visibility.Collapsed;
            }
        }
    }
}
