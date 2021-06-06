using System.Collections.Generic;
using System.ComponentModel;

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages
{
    public interface IPage <T> : INotifyPropertyChanged
    {
        void RefreshGrid();
        List<T> DataSource { get; set; }
    }
}