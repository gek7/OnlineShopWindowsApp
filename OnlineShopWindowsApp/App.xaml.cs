using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OnlineShopWindowsApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void templateGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            (sender as Grid).Margin = new Thickness(7);
        }
    }
}
