using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LeftBarMenu.xaml
    /// </summary>
    public partial class LeftBarMenu : UserControl, INotifyPropertyChanged
    {
        private List<Category> _categories;
        public Popup ShowedPopup;
        public static List<LeftBarMenu> LeftBarMenus = new List<LeftBarMenu>();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public List<Category> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }
        public LeftBarMenu()
        {
            InitializeComponent();
            LeftBarMenus.Add(this);
            PropertyChanged += LeftBarMenu_PropertyChanged;
        }

        private void LeftBarMenu_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Categories" && Categories != null && Categories.Count > 0)
            {
                Button btnSender = (sender as Button);
                rootPanel.Children.Clear();
                Categories.ForEach(c =>
                {
                    Button btn = new Button();
                    btn.MouseEnter += CategoryEnter;
                    btn.MouseLeave += CategoryLeave;
                    btn.Content = c.name;
                    btn.DataContext = c;
                    rootPanel.Children.Add(btn);
                });
            }
        }

        private void EnterFirst(object sender, MouseEventArgs e)
        {
            //var p = new Popup() { Child = new Button() { Content = "2" }, StaysOpen = false, Margin = new Thickness(200, 0, 0, 0), Placement=PlacementMode.Right, PlacementTarget = rootBtn};
            //p.IsOpen = true;
            //roo.Children.Add(p);
        }

        private void CategorySelect(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn != null)
            {
                Category selectCategory = btn.DataContext as Category;
                if (DataContext != null && selectCategory != null)
                {
                    MainWindow.mainWindow.mainFrame.Navigate(new ItemsPage(selectCategory.id, selectCategory.name));
                }
            }
        }

        private void CategoryEnter(object sender, MouseEventArgs e)
        {
            Debug.Print($"enter {(sender as Button).Content as String}");
            Button btn = e.Source as Button;
            if (btn != null)
            {
                Category selectCategory = btn.DataContext as Category;
                if (DataContext != null && selectCategory != null && selectCategory.categories != null && selectCategory.categories.Count > 0)
                {
                    //Показ подкатегорий
                    var p = new Popup() { StaysOpen = true, Margin = new Thickness(0, 0, 0, 0), Placement = PlacementMode.Right, PlacementTarget = btn };
                    p.AllowsTransparency = true;
                    p.IsOpen = true;
                    var subCategories = new LeftBarMenu();
                    subCategories.Categories = selectCategory.categories;
                    p.Child = subCategories;
                    this.ShowedPopup = p;
                    MainWindow.mainWindow.PopupGrid.Children.Add(p);
                }
            }
        }

        private void CategoryLeave(object sender, MouseEventArgs e)
        {
            Debug.Print($"leave {(sender as Button).Content as String}");
            Button btn = e.Source as Button;
            if (btn != null)
            {
                Category selectCategory = btn.DataContext as Category;
                if (DataContext != null && selectCategory != null)
                {
                    LeftBarMenu MouseOverMenu = LeftBarMenus.Where(menu => menu.IsMouseOver).FirstOrDefault();
                    if (MouseOverMenu != null)
                    {
                        Button MouseOverItem = (Button)MouseOverMenu.rootPanel.Children.Cast<UIElement>().Where(el => el.IsMouseOver).FirstOrDefault();
                        List<Popup> PopupForDelete = new List<Popup>();
                        Popup curPopup = MouseOverMenu.ShowedPopup;
                        if (MouseOverMenu.ShowedPopup != null && MouseOverMenu.ShowedPopup.Child is LeftBarMenu && MouseOverMenu != this)
                        {
                            curPopup = (MouseOverMenu.ShowedPopup.Child as LeftBarMenu).ShowedPopup;
                        }
                        while (curPopup != null)
                        {
                            PopupForDelete.Add(curPopup);
                            LeftBarMenu curMenu = curPopup.Child as LeftBarMenu;
                            curPopup = curMenu.ShowedPopup;
                        }
                        PopupForDelete.ForEach(p =>
                        {
                            LeftBarMenus.Remove((LeftBarMenu)p.Child);
                            MainWindow.mainWindow.PopupGrid.Children.Remove(p);
                        });
                    }
                    else
                    {
                        //Удаление всех всплывающих категорий
                        LeftBarMenus.Clear();
                        var children = MainWindow.mainWindow.PopupGrid.Children;
                        for (int i = 0; i < children.Count; i++)
                        {
                            if (children[i] is Popup)
                            {
                                LeftBarMenus.Remove( LeftBarMenus.Where(item => item.ShowedPopup == children[i]).FirstOrDefault());
                                (children[i] as Popup).IsOpen = false;
                            }
                        }
                        children.Clear();
                    }
                }
            }
        }
    }
}


