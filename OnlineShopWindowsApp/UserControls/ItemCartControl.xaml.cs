using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;
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
    /// Логика взаимодействия для ItemCartControl.xaml
    /// </summary>
    public partial class ItemCartControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SourceChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public void OnSourceChanged()
        {
            if ( SourceChanged != null)
                SourceChanged(this, null);
        }
        private bool _isWishItem;
        public bool isWishItem
        {
            get
            {
                return _isWishItem;
            }
            set
            {
                _isWishItem = value;
                if (value)
                {
                    AddToCart.Visibility = Visibility.Visible;
                    counter.Visibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }
        private bool _isOnlyInfo;
        public bool IsOnlyInfo
        {
            get
            {
                return _isOnlyInfo;
            }
            set
            {
                _isOnlyInfo = value;
                if (value)
                {
                    deleteBtn.Visibility = Visibility.Collapsed;
                    counter.Visibility = Visibility.Collapsed;
                    infoCount.Visibility = Visibility.Visible;
                }
                else
                {
                    deleteBtn.Visibility = Visibility.Visible;
                    counter.Visibility = Visibility.Visible;
                    infoCount.Visibility = Visibility.Collapsed;
                }
                OnPropertyChanged();
            }
        }
        public ItemCartControl()
        {
            InitializeComponent();
        }

        public ItemCartControl(bool isWish, bool isOnlyInfo) : this()
        {
            this.isWishItem = isWish;
            this.IsOnlyInfo = isOnlyInfo;
        }

        private void AddCount(object sender, RoutedEventArgs e)
        {
            ItemCart ic = (this.DataContext as ItemCart);
            (this.DataContext as ItemCart).Count++;
            MainWindow.mainWindow.AddItemToCart(ic.id);
            OnSourceChanged();
        }

        private void SubCount(object sender, RoutedEventArgs e)
        {
            ItemCart ic = (this.DataContext as ItemCart);
            ic.Count--;
            MainWindow.mainWindow.SubItemFromCart(ic.id);
            if (Parent is StackPanel && ic.Count <= 0)
            {
                (Parent as StackPanel).Children.Remove(this);
            }
            OnSourceChanged();
        }

        private void DeleteCartItem(object sender, RoutedEventArgs e)
        {
            DeleteItem();
        }

        public void DeleteItem()
        {

            ItemCart ic = (this.DataContext as ItemCart);
            while (ic.Count > 0)
            {
                ic.Count--;
                MainWindow.mainWindow.SubItemFromCart(ic.id);
            }
            if (Parent is StackPanel && ic.Count <= 0)
            {
                (Parent as StackPanel).Children.Remove(this);
            }
            OnSourceChanged();
        }
    }
}
