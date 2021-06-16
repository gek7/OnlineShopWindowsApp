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
            //(sender as Grid).Margin = new Thickness(7);
        }

        private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window w = sender as Window;
            if (w.WindowState == WindowState.Maximized)
            {
                w.WindowState = WindowState.Normal;
                w.Left = 0;
                w.Top = 0;
                w.Width = SystemParameters.PrimaryScreenWidth;
                w.Height = SystemParameters.PrimaryScreenHeight;
            }
        }
    }
}
