using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace OnlineShopWindowsApp
{
    public static class HelperClass
    {
        //Запуск анимации загрузки на кнопке
        public static void SetButtonIndicator(ButtonBase btn, bool value)
        {
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, value);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, value);
        }

        public static void ShowWaiting()
        {
            //MainWindow.mainWindow.
        }
    }
}
