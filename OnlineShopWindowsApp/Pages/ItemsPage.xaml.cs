using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using OnlineShopWindowsApp.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace OnlineShopWindowsApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ItemsPage.xaml
    /// </summary>
    public partial class ItemsPage : Page, INotifyPropertyChanged
    {
        public long ItemsCategoryId { get; set; }
        private ObservableCollection<Item> _productSource { get; set; }
        public ObservableCollection<Item> ProductSource
        {
            get
            {
                return _productSource;
            }
            set
            {
                _productSource = value;
                for (int i = 0; i < value.Count; i++)
                {
                    ItemControl ic = new ItemControl();
                    ic.DataContext = value[i];
                    ItemsPanel.Children.Add(ic);
                }
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ItemsPage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public ItemsPage(long id):this()
        {
            ItemsCategoryId = id;
            RefreshDataSource();
        }

        private async void RefreshDataSource()
        {
            var src = await RequestsHelper.GetRequest<ObservableCollection<Item>>($"{MainWindow.BaseAddress}/api/items/getAllItemsByCategory?id={ItemsCategoryId}", true);
            ProductSource = src.Obj;
        }

    }
}
