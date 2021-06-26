using Microsoft.Win32;
using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ItemDialog.xaml
    /// </summary>
    public partial class ItemDialog : Window, INotifyPropertyChanged
    {
        private Item _selectedObj = new Item() { owner = new Item(), category = new Category() };
        private ItemImage _currentImage;
        private ObservableCollection<Image> _imagesSources = new ObservableCollection<Image>();
        private List<ItemImage> NewImagePaths = new List<ItemImage>();

        public ObservableCollection<Image> ImagesSources
        {
            get
            {
                return _imagesSources;
            }
            set
            {
                _imagesSources = value;
                OnPropertyChanged();
            }
        }

        public Item SelectedObj
        {
            get
            {
                return _selectedObj;
            }
            set
            {
                _selectedObj = value;
                HelperClass.SetXaml(tbDescription, value.description);
                OnPropertyChanged();
            }
        }

        public Image CurrentImage
        {
            get
            {
                if (ImagesControl.Items.Count == 0) return null;
                return (Image)ImagesControl.Items[ImagesControl.PageIndex];
            }
        }

        public ItemImage CurrentItemImage
        {
            get
            {
                if(ImagesControl.Items.Count > 0)
                {
                    var ItmImg = CurrentImage.DataContext as ItemImage;
                    if (ItmImg is ItemImage)
                    {
                        return ItmImg as ItemImage;
                    }
                }
                return null;
            }
            set
            {
                _currentImage = value;
                OnPropertyChanged();
            }
        }
        public ActionType ActionKind { get; set; }
        public ItemDialog(ActionType type, long id = -1)
        {
            InitializeComponent();
            ImagesControl.ItemsSource = ImagesSources;
            DataContext = this;
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

            await RequestsHelper.GetRequest<List<Item>>($"{MainWindow.BaseAddress}/api/Items/getAllItems")
                  .ContinueWith((response) =>
                  {
                      OwnerCmb.ItemsSource = response.Result.Obj;
                      OwnerCmb.DisplayMemberPath = "name";
                  }, TaskScheduler.FromCurrentSynchronizationContext());
            await RequestsHelper.GetRequest<List<Category>>($"{MainWindow.BaseAddress}/api/Categories/getAllCategories")
                      .ContinueWith(async (response) =>
                      {
                          CategoriesCmb.ItemsSource = response.Result.Obj;
                          CategoriesCmb.DisplayMemberPath = "name";

                          if (ActionKind == ActionType.Edit)
                          {
                              var Item = await RequestsHelper.GetRequest<Item>($"{MainWindow.BaseAddress}/api/Items?id={id}", true);
                              SelectedObj = Item.Obj;
                              if (SelectedObj.images != null && SelectedObj.images.Count > 0)
                              {
                                  List<Image> imgs = new List<Image>();
                                  for (int i = 0; i < SelectedObj.images.Count; i++)
                                  {
                                      Image im = new Image();
                                      ItemImage img = SelectedObj.images[i];
                                      byte[] arr = await RequestsHelper.getByteArray(img.path);
                                      im.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(arr);
                                      im.DataContext = img;
                                      ImagesSources.Add(im);
                                  }
                              }
                              ImagesControl.UpdatePageButtons();
                              if(CurrentImage != null)
                              CurrentItemImage = CurrentImage.DataContext as ItemImage;
                          }

                      }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {

            Close();
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            SelectedObj.description = HelperClass.GetXaml(tbDescription);
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
                AdvanceResponse<Item> response;

                if (ActionKind == ActionType.Add)
                {
                    response = await RequestsHelper.PutRequest<Item>($"{MainWindow.BaseAddress}/api/Items", SelectedObj);
                    if (NewImagePaths.Count != 0 && response.SourceResponse.IsSuccessStatusCode)
                        for (int i = 0; i < NewImagePaths.Count; i++)
                        {
                            await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/Items/setImage?id={response.Obj.id}&isMain={NewImagePaths[i].isMain}", NewImagePaths[i].path);
                        }

                }
                else
                {
                    response = await RequestsHelper.PostRequest<Item>($"{MainWindow.BaseAddress}/api/Items", SelectedObj);
                    if (NewImagePaths.Count != 0 && response.SourceResponse.IsSuccessStatusCode)
                        for (int i = 0; i < NewImagePaths.Count; i++)
                        {
                            await RequestsHelper.SendFile($"{MainWindow.BaseAddress}/api/Items/setImage?id={response.Obj.id}&isMain={NewImagePaths[i].isMain}", NewImagePaths[i].path);
                        }
                }

                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    var FrameContent = MainWindow.mainWindow.mainFrame.Content as ItemsPage;
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
            ofd.Multiselect = true;
            ofd.Filter = "pictures |*.jpg;*.png";
            if (ofd.ShowDialog() == true)
            {
                ofd.FileNames.ToList().ForEach(path =>
                {
                    ItemImage itImg = new ItemImage();
                    itImg.path = path;
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri(itImg.path));
                    img.DataContext = itImg;
                    NewImagePaths.Add(itImg);
                    ImagesSources.Add(img);
                    ImagesControl.UpdatePageButtons();
                });
            }
        }

        private void DeleteImage(object sender, RoutedEventArgs e)
        {
            if(CurrentImage != null)
            {
                if(SelectedObj.images != null)
                {
                    var img = SelectedObj.images.Where(i => i.id == CurrentItemImage.id).FirstOrDefault();
                    if (img != null)
                    {
                        SelectedObj.images.Remove(img);
                        ImagesSources.Remove(CurrentImage);
                        ImagesControl.UpdatePageButtons();
                    }
                }
                if(NewImagePaths != null)
                {
                    var img = NewImagePaths.Where(i => i.id == CurrentItemImage.id).FirstOrDefault();
                    if (img != null)
                    {
                        NewImagePaths.Remove(img);
                        ImagesSources.Remove(CurrentImage);
                        ImagesControl.UpdatePageButtons();
                    }
                }
            }

        }

        private void ChangeImage(object sender, RoutedEventArgs e)
        {
            if(CurrentImage != null)
            {
                var context = CurrentImage.DataContext;
                if (context is ItemImage)
                {
                    CurrentItemImage = context as ItemImage;
                }
            }
        }
    }
}
