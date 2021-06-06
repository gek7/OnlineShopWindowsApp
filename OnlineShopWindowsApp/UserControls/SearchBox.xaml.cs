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
    
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
            cbSearch.Items.Add("dada");
            cbSearch.Items.Add("dada");
            cbSearch.Items.Add("dada");
            cbSearch.Items.Add("dada");
            cbSearch.Items.Add("dada");
            cbSearch.Items.Add("dada");
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            cbSearch.IsDropDownOpen = true;
            tbSearch.Focus();
        }

        private void cbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbSearch.Text = cbSearch.SelectedItem.ToString();
        }

        private void cbSearch_DropDownOpened(object sender, EventArgs e)
        {
           tbSearch.Focus();
        }

        private void cbSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            cbSearch.IsDropDownOpen = true;
        }
    }
}
