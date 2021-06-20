using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ReviewControl.xaml
    /// </summary>
    public partial class ReviewControl : UserControl, INotifyPropertyChanged
    {
        public StackPanel ReviewsPanel { get; set; }
        private object chipIconContent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ReviewControl()
        {
            InitializeComponent();
        }
        public ReviewControl(StackPanel reviewsPanel) : this()
        {
            ReviewsPanel = reviewsPanel;
        }

        private async void ClickForAdmin(object sender, MouseEventArgs e)
        {
            Review r = this.DataContext as Review;
            if (r != null)
            {
                if (HelperClass.isAuth() && (MainWindow.User.role.id == 1 || MainWindow.User.id == r.user.id))
                {

                    var reviews = await RequestsHelper.DeleteRequest<List<Review>>($"{MainWindow.BaseAddress}/api/items/reviews?ReviewId={r.id}");
                    if (reviews.SourceResponse.IsSuccessStatusCode)
                    {
                        ReviewsPanel.Children.Remove(this);
                    }
                }
            }
        }

        private void LeaveForAdmin(object sender, MouseEventArgs e)
        {
            Review r = this.DataContext as Review;
            if (r != null)
            {
                if (HelperClass.isAuth() && (MainWindow.User.role.id == 1 || MainWindow.User.id == r.user.id))
                {
                    ReviewerChip.Icon = chipIconContent;
                }
            }
        }

        private void EnterForAdmin(object sender, MouseEventArgs e)
        {
            Review r = this.DataContext as Review;
            if (r != null)
            {
                if (HelperClass.isAuth() && MainWindow.User != null && 
                    (MainWindow.User.id == r.user.id || 
                    (MainWindow.User.role != null && MainWindow.User.role.id == 1)))
                {
                    chipIconContent = ReviewerChip.Icon;
                    PackIcon pi = new PackIcon() { Kind = PackIconKind.Delete };
                    pi.Cursor = Cursors.Hand;
                    pi.MouseDown += ClickForAdmin;
                    ReviewerChip.Icon = pi;
                }
            }
        }
    }
}
