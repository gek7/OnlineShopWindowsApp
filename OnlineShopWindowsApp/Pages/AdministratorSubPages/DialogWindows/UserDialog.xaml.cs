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
using System.Text.Json;
using Microsoft.Win32;
using System.Net.Http;

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window, INotifyPropertyChanged
    {
        private User _selectedObj = new User();
        private string NewImagePath { get; set; }
        public User SelectedObj
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
        public UserDialog(ActionType type, long id = -1)
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
            await RequestsHelper.GetRequest<List<Role>>($"{MainWindow.BaseAddress}/api/users/roles", true)
                  .ContinueWith(async (response) =>
                  {
                      rolesCmb.ItemsSource = response.Result.Obj;
                      rolesCmb.DisplayMemberPath = "name";
                      rolesCmb.SelectedValuePath = "id";
                      //Если меняем информацию о пользователе, то узнаем текущую информацию
                      if (ActionKind == ActionType.Edit)
                      {
                          var user = await RequestsHelper.GetRequest<User>($"{MainWindow.BaseAddress}/api/users?id={id}", true);
                          SelectedObj = user.Obj;
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
            if (String.IsNullOrWhiteSpace(SelectedObj.firstName))
            {
                errors += "Заполните имя\n";
            }
            if (String.IsNullOrWhiteSpace(SelectedObj.lastName))
            {
                errors += "Заполните фамилию\n";
            }
            if (rolesCmb.SelectedItem == null)
            {
                errors += "Укажите роль\n";
            }

            if (ActionKind == ActionType.Add)
            {
                if (String.IsNullOrWhiteSpace(SelectedObj.login))
                {
                    errors += "Заполните логин\n";
                }
                if (String.IsNullOrWhiteSpace(SelectedObj.login))
                {
                    errors += "Заполните пароль\n";
                }
            }

            if (errors != "")
            {
                MessageBox.Show(errors);
                return;
            }
            else
            {
                AdvanceResponse<User> response;
                //отправка запроса на сервер
                if(NewImagePath != null)
                {
                    //Отправка картинки на сервер 
                    //http://localhost:5000/api/users/setImage?id=10
                }

                if (ActionKind == ActionType.Add)
                {
                    response = await RequestsHelper.PutRequest<User>($"{MainWindow.BaseAddress}/api/users", SelectedObj);
                    if (NewImagePath != null)
                        await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/users/setImage?id={response.Obj.id}", NewImagePath);

                }
                else
                {
                    if (NewImagePath != null)
                        await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/users/setImage?id={SelectedObj.id}", NewImagePath);
                    response = await RequestsHelper.PostRequest<User>($"{MainWindow.BaseAddress}/api/users", SelectedObj);
                }

                if (!response.SourceResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show(response.SourceResponse.ReasonPhrase);
                }
                else
                {
                    var FrameContent = MainWindow.mainWindow.mainFrame.Content as UsersPage;
                    if (FrameContent != null)
                    {
                        FrameContent.RefreshGrid();
                    }
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
            }
            curImg.Source = new BitmapImage(new Uri(NewImagePath));
        }

        private void DeleteImage(object sender, RoutedEventArgs e)
        {
            _selectedObj.image = null;
            NewImagePath = null;
            curImg.Source =(ImageSource) new ImageSourceConverter().ConvertFrom("/accountImg.png");
        }
    }
}
