﻿using OnlineShopWindowsApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для SignInControl.xaml
    /// </summary>
    public partial class SignInControl : UserControl
    {
        public Action regClick{ get; set; }
        public Action CallBack { get; set; }
        public SignInControl()
        {
            InitializeComponent();
        }
        private void RegClick(object sender, RoutedEventArgs e)
        {
            regClick?.Invoke();
        }
        private void loginClick(object sender, RoutedEventArgs e)
        {
            //Проверка

            CallBack?.Invoke();
        }
    }
}
