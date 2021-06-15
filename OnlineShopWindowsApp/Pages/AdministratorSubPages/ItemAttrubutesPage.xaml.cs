using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows;
using OnlineShopWindowsApp.ServerActions;
using OnlineShopWindowsApp.UserControls;
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
    /// Логика взаимодействия для ItemAttrubutesPage.xaml
    /// </summary>
    public partial class ItemAttrubutesPage : Page, IPage<ItemAttribute>
    {
        DataGrid ItemsGrid { get; set; }
        public Item CurrentItem
        {
            get => ItemsGrid.SelectedItem as Item;
        }
        private List<ItemAttribute> _dataSource;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public List<ItemAttribute> DataSource
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

        public ItemAttrubutesPage(ItemsPage page)
        {
            InitializeComponent();
            if (page != null) ItemsGrid = page.dataGrid;
            this.DataContext = this;
            page.dataGrid.SelectionChanged += DataGrid_SelectionChanged; ;
            FillAttrsByItem();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillAttrsByItem();
        }

        private async void FillAttrsByItem()
        {

            if (CurrentItem != null)
            {
                var attrs = await RequestsHelper.GetRequest<List<ItemAttribute>>($"{MainWindow.BaseAddress}/api/items/ItemAttributes?itemId={CurrentItem.id}");
                if (attrs.SourceResponse.IsSuccessStatusCode)
                {
                    DataSource = attrs.Obj;
                }
            }
        }

        private async void DeleteAttr(object sender, RoutedEventArgs e)
        {
            if (attrGrid.SelectedItem != null)
            {
                var result = await MainWindow.ExecuteBoolDialog(new DeleteQuestion());

                if (result)
                {
                    var response = await RequestsHelper.DeleteRequest<ItemAttribute>($"{MainWindow.BaseAddress}/api/items/ItemAttribute?id={((ItemAttribute)attrGrid.SelectedItem).id}", true);
                    if (response.SourceResponse.IsSuccessStatusCode)
                    {
                        FillAttrsByItem();
                    }
                }
            }
        }

        public void RefreshGrid()
        {
            FillAttrsByItem();
        }

        private void AddAttr(object sender, RoutedEventArgs e)
        {
            if (ItemsGrid.SelectedItem != null)
            {
                new ItemAttributeDialog(ActionType.Add, CurrentItem.id, CurrentItem.category.id, -1).ShowDialog();
            }
        }

        private void EditAttr(object sender, RoutedEventArgs e)
        {
            if (attrGrid.SelectedItem != null)
            {
                new ItemAttributeDialog(ActionType.Edit, CurrentItem.id, CurrentItem.category.id, (attrGrid.SelectedItem as ItemAttribute).id).ShowDialog();
            }
        }
    }
}
