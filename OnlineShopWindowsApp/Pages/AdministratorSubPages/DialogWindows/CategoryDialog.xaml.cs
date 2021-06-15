using Microsoft.Win32;
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
    /// Логика взаимодействия для CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : Window, INotifyPropertyChanged
    {
        private Category _selectedObj = new Category() {owner = new Category() };
        private string NewImagePath { get; set; }
        public Category SelectedObj
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
        public CategoryDialog(ActionType type, long id = -1)
        {
            InitializeComponent();
            DataContext = this;
            NewImagePath = null;
            ActionKind = type;
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
            await RequestsHelper.GetRequest<List<Category>>($"{MainWindow.BaseAddress}/api/categories/getAllCategories")
                  .ContinueWith(async (response) =>
                  {
                      rolesCmb.ItemsSource = response.Result.Obj;
                      rolesCmb.DisplayMemberPath = "name";
                      
                      if (ActionKind == ActionType.Edit)
                      {
                          var Category = await RequestsHelper.GetRequest<Category>($"{MainWindow.BaseAddress}/api/Categories?id={id}", true);
                          SelectedObj = Category.Obj;
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
                AdvanceResponse<Category> response;

                if (ActionKind == ActionType.Add)
                {
                    response = await RequestsHelper.PutRequest<Category>($"{MainWindow.BaseAddress}/api/Categories", SelectedObj);
                    if (NewImagePath != null && response.SourceResponse.IsSuccessStatusCode)
                        await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/Categories/setImage?id={response.Obj.id}", NewImagePath);

                }
                else
                {
                    response = await RequestsHelper.PostRequest<Category>($"{MainWindow.BaseAddress}/api/Categories", SelectedObj);
                    if (NewImagePath != null && response.SourceResponse.IsSuccessStatusCode)
                        await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/Categories/setImage?id={response.Obj.id}", NewImagePath);
                }

                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    var FrameContent = MainWindow.mainWindow.mainFrame.Content as CategoriesPage;
                    if (FrameContent != null)
                    {
                        FrameContent.RefreshGrid();
                    }
                    MainWindow.mainWindow.FillCategories();
                    Close();
                }
            }
        }

        private void ChooseImage(object sender, RoutedEventArgs e)
        {
            //Выбор новой картинки
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                NewImagePath = ofd.FileName;
                curImg.Source = new BitmapImage(new Uri(NewImagePath));
            }
        }

        private void DeleteImage(object sender, RoutedEventArgs e)
        {
            _selectedObj.image = null;
            NewImagePath = null;
            curImg.Source = null;
        }

        private void ResetParent(object sender, RoutedEventArgs e)
        {
            SelectedObj.owner = null;
            rolesCmb.SelectedIndex = -1;
        }
    }
}
