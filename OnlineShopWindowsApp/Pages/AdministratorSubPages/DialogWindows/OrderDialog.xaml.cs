using OnlineShopWindowsApp.Models;
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
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для OrderDialog.xaml
    /// </summary>
    public partial class OrderDialog : Window, INotifyPropertyChanged
    {
        private long selectedId;

        public OrderDialog(long id = -1)
        {
            InitializeComponent();
            DataContext = this;
            selectedId = id;
            FillFields(id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public async void FillFields(long id)
        {
            await RequestsHelper.GetRequest<List<OrderStatus>>($"{MainWindow.BaseAddress}/api/orders/getStatuses")
                  .ContinueWith((response) =>
                  {
                      statusCmb.ItemsSource = response.Result.Obj;
                      statusCmb.DisplayMemberPath = "name";
                  }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            if (statusCmb.SelectedItem != null)
            {
                var response = await RequestsHelper.PostRequest<List<OrderStatus>>(
                $"{MainWindow.BaseAddress}/api/orders/EditStatus?orderId={selectedId}&newStatusId={((OrderStatus)statusCmb.SelectedItem).id}");
                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    var FrameContent = MainWindow.mainWindow.mainFrame.Content as OrdersPage;
                    if (FrameContent != null)
                    {
                        FrameContent.RefreshGrid();
                    }
                    Close();
                }
            }
            else
            {
                HelperClass.message("Заполните статуc");
            }
        }
    }
}
