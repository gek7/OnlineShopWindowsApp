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
    /// Логика взаимодействия для ItemPage.xaml
    /// </summary>
    public partial class ItemPage : Page, INotifyPropertyChanged
    {
        private Item _item;

        private ObservableCollection<Image> _imagesSources = new ObservableCollection<Image>();
        public event PropertyChangedEventHandler PropertyChanged;
        public Item item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                if(value.description != null)
                HelperClass.SetXaml(tbDescription, value.description);
                OnPropertyChanged();
            }
        }

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

       
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ItemPage()
        {
            InitializeComponent();
            DataContext = this;
            if (HelperClass.isAuth())
            {
                ReviewForm.Visibility = Visibility.Visible;
            }
        }

        public ItemPage(long id) : this()
        {
            FillItem(id);
        }

        public async void FillItem(long id)
        {
            var Item = await RequestsHelper.GetRequest<Item>($"{MainWindow.BaseAddress}/api/Items?id={id}", true);
            if (Item.SourceResponse.IsSuccessStatusCode)
            {
                item = Item.Obj;
                if (item.images != null && item.images.Count > 0)
                {
                    ImagesSources.Clear();
                    List<Image> imgs = new List<Image>();
                    for (int i = 0; i < item.images.Count; i++)
                    {
                        Image im = new Image();
                        ItemImage img = item.images[i];
                        byte[] arr = await RequestsHelper.getByteArray(img.path);
                        im.Source = (ImageSource)new ImageSourceConverter().ConvertFrom(arr);
                        im.DataContext = img;
                        ImagesSources.Add(im);
                    }
                    ImagesControl.ItemsSource = ImagesSources;
                    ImagesControl.UpdatePageButtons(0);
                }
                RefreshReviews();
            }
            var response = await RequestsHelper.GetRequest<List<ItemAttribute>>($"{MainWindow.BaseAddress}/api/items/ItemAttributes?itemId={id}", true);
            if (Item.SourceResponse.IsSuccessStatusCode)
            {
                List<ItemAttribute> attrs = response.Obj;
                if (attrs.Count > 0)
                {
                    AttributePanelForControl.Children.Clear();
                    foreach (var item in attrs)
                    {
                        AttrControl ac = new AttrControl();
                        ac.DataContext = item;
                        AttributePanelForControl.Children.Add(ac);
                    }
                }
                else
                {
                    AttributePanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void RefreshReviews()
        {
            var reviews = await RequestsHelper.GetRequest<List<Review>>($"{MainWindow.BaseAddress}/api/items/reviews?ItemId={item.id}", false);
            if (reviews.SourceResponse.IsSuccessStatusCode)
            {
                ReviewsPanel.Children.Clear();
                reviews.Obj.ForEach(r =>
                {
                    ReviewControl rc = new ReviewControl(ReviewsPanel);
                    rc.DataContext = r;
                    ReviewsPanel.Children.Add(rc);
                });
            }
        }

        private void AddToCart(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.AddItemToCart(item.id);
        }

        private async void SendReview(object sender, RoutedEventArgs e)
        {
            if (ReviewStars.Value == 0 && string.IsNullOrWhiteSpace(ReviewContent.Text))
            {
                MainWindow.mainWindow.MainSnackbar
                    .MessageQueue?.Enqueue("Должен быть проставлен рейтинг или написан отзыв");
                return;
            }
            Review r = new Review();
            r.itemMark = ReviewStars.Value;
            r.reviewContent = ReviewContent.Text;
            r.item = item.id;
           var result = await RequestsHelper.PutRequest<Review>($"{MainWindow.BaseAddress}/api/items/reviews", r);
            if (result.SourceResponse.IsSuccessStatusCode)
            {
                ClearReviewFields();
                FillItem(item.id);
                RefreshReviews();
            }
        }

        private void ClearFields(object sender, RoutedEventArgs e)
        {
            ClearReviewFields();
        }

        private void ClearReviewFields()
        {
            ReviewStars.Value = 0;
            ReviewContent.Text = "";
        }

        private void AddToWishlist(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.AddItemToWishList(item.id);
        }
    }
}
