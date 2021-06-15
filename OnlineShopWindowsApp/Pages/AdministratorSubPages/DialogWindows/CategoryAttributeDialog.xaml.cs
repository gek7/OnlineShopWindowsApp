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
    /// Логика взаимодействия для CategoryAttributeAttributeDialog.xaml
    /// </summary>
    public partial class CategoryAttributeDialog : Window, INotifyPropertyChanged
    {
        private CategoryAttributeModel _selectedObj = new CategoryAttributeModel() { attrType = new CategoryAttributeType() };
        public CategoryAttributeModel SelectedObj
        {
            get
            {
                return _selectedObj;
            }
            set
            {
                _selectedObj = value;
                OnPropertyChanged();
            }
        }
        public ActionType ActionKind { get; set; }
        public CategoryAttributeDialog(ActionType type, long id = -1, long categoryId = -1)
        {
            InitializeComponent();
            DataContext = this;
            ActionKind = type;
            SelectedObj.category = categoryId;
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
            await RequestsHelper
                .GetRequest<List<CategoryAttributeType>>($"{MainWindow.BaseAddress}/api/categories/CategoryAttributes/types")  
                  .ContinueWith(async (response) =>
                  {
                      typesCmb.ItemsSource = response.Result.Obj;
                      typesCmb.DisplayMemberPath = "name";

                      if (ActionKind == ActionType.Edit)
                      {
                          var attr = await RequestsHelper
                          .GetRequest<CategoryAttributeModel>($"{MainWindow.BaseAddress}/api/categories/CategoryAttribute?attrId={id}", true);
                          SelectedObj = attr.Obj;
                      }
                  }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            string errors = "";
            if (String.IsNullOrWhiteSpace(SelectedObj.name))
            {
                errors += "Заполните наименование\n";
            }

            if (errors != "")
            {
                HelperClass.message(errors);
                return;
            }
            else
            {
                AdvanceResponse<CategoryAttributeModel> response;

                if (ActionKind == ActionType.Add)
                {
                    response = await RequestsHelper.PutRequest<CategoryAttributeModel>($"{MainWindow.BaseAddress}/api/categories/CategoryAttribute", SelectedObj);

                }
                else
                {
                    response = await RequestsHelper.PostRequest<CategoryAttributeModel>($"{MainWindow.BaseAddress}/api/categories/CategoryAttribute", SelectedObj);
                }

                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    var FrameContent = MainWindow.mainWindow.additionalFrame.Content as IPage<CategoryAttributeModel>;
                    if (FrameContent != null)
                    {
                        FrameContent.RefreshGrid();
                    }
                    Close();
                }
            }
        }
    }
}
