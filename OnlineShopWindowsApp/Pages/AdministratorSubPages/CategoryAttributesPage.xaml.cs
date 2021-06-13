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
    /// Логика взаимодействия для CategoryAttributesPage.xaml
    /// </summary>
    public partial class CategoryAttributesPage : Page, IPage<CategoryAttributeModel>, INotifyPropertyChanged
    {
        DataGrid CategoryGrid { get; set; }
        public Category CurrentCategory
        {
            get => CategoryGrid.SelectedItem as Category;
        }
        private List<CategoryAttributeModel> _dataSource;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public List<CategoryAttributeModel> DataSource
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

        public CategoryAttributesPage(CategoriesPage page)
        {
            InitializeComponent();
            if (page != null) CategoryGrid = page.dataGrid;
            this.DataContext = this;
            page.PropertyChanged += CategoryChanged;
            FillAttrsByCategory();
        }

        private void CategoryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FillAttrsByCategory();
        }

        private async void FillAttrsByCategory()
        {

            if(CurrentCategory != null)
            {
                var attrs = await RequestsHelper.GetRequest<List<CategoryAttributeModel>>($"{MainWindow.BaseAddress}/api/categories/CategoryAttributes?categoryId={CurrentCategory.id}");
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
                var response = await RequestsHelper.DeleteRequest<CategoryAttributeModel>($"{MainWindow.BaseAddress}/api/categories/CategoryAttribute?id={((CategoryAttributeModel)attrGrid.SelectedItem).id}", true);
                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    FillAttrsByCategory();
                }
            }
        }

        public void RefreshGrid()
        {
            FillAttrsByCategory();
        }

        private void AddAttr(object sender, RoutedEventArgs e)
        {
            if (CategoryGrid.SelectedItem != null)
            {
                new CategoryAttributeDialog(ActionType.Add, -1, (CategoryGrid.SelectedItem as Category).id).ShowDialog();
            }
        }

        private void EditAttr(object sender, RoutedEventArgs e)
        {
            if(attrGrid.SelectedItem != null)
            {
                new CategoryAttributeDialog(ActionType.Edit, (attrGrid.SelectedItem as CategoryAttributeModel).id).ShowDialog();
            }
        }
    }
}
