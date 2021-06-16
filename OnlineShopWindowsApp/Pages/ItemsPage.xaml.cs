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
        private List<UserControl> filters = new List<UserControl>();
        public ObservableCollection<Item> ProductSource
        {
            get
            {
                return _productSource;
            }
            set
            {
                _productSource = value;
                ItemsPanel.Children.Clear();
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
        public ItemsPage(long id, string categoryName) : this()
        {
            tbCategoryName.Text = categoryName;
            ItemsCategoryId = id;
            RefreshDataSource();
        }

        private async void RefreshDataSource()
        {

            ProductSource = await getNewDataSource();
            FillPanelForFilter();
        }

        private void FillPanelForFilter()
        {
            panelForFilterControls.Children.Clear();
            var minPrice = ProductSource.Min(i => i.price);
            var maxPrice = ProductSource.Max(i => i.price);
            RangeControl rc = new RangeControl(minPrice, maxPrice);
            filters.Add(rc);
            panelForFilterControls.Children.Add(rc);

        }

        private async void ApplyFilter(object sender, RoutedEventArgs e)
        {
            if (filters.Count > 0)
            {
                var source = await getNewDataSource();

                var priceFilter = filters[0] as RangeControl;
                var newSrc = source.Where(item => (double)item.price >= priceFilter.Slider.ValueStart && (double)item.price <= priceFilter.Slider.ValueEnd);
                switch (cmbSort.SelectedIndex)
                {
                    case 0:
                        newSrc = newSrc.OrderBy(item => item.price);
                        break;
                    case 1:
                        newSrc = newSrc.OrderByDescending(item => item.avgRating);
                        break;
                    case 2:
                        newSrc = newSrc.OrderBy(item => item.name);
                        break;
                }
                ProductSource = new ObservableCollection<Item>(newSrc);
            }
        }

        private async Task<ObservableCollection<Item>> getNewDataSource()
        {
            var result = await RequestsHelper.GetRequest<ObservableCollection<Item>>($"{MainWindow.BaseAddress}/api/items/getAllItemsByCategory?id={ItemsCategoryId}", true);
            return result.Obj;
        }

        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter(this, null);
        }
    }
}
