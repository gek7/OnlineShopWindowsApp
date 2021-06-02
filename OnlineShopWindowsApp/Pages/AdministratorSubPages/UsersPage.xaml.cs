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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page, INotifyPropertyChanged
    {
        private List<User> _dataSource;
        public List<User> DataSource
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

        public UsersPage()
        {
            InitializeComponent();
            this.DataContext = this;
            RefreshGrid();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public async void RefreshGrid()
        {
            var task = RequestsHelper.GetRequest<List<User>>($"{MainWindow.BaseAddress}/api/users/getAllUsers", true);
            await task.ContinueWith((previous) =>
            {
                List<User> response = previous.Result.Obj.ToList();
                DataSource = response.ToList();
                this.DataContext = this;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            new UserDialog(ActionType.Add).ShowDialog();
        }

        private async void Delete(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                 await RequestsHelper.DeleteRequest<User>($"{MainWindow.BaseAddress}/api/users?id={((User)dataGrid.SelectedItem).id}", true);
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
                new UserDialog(ActionType.Edit, ((User)dataGrid.SelectedItem).id).ShowDialog();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshGrid();
        }
    }
}
