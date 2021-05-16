using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace OnlineShopWindowsApp
{
    public static class HelperClass
    {
        public static void SetButtonIndicator(ButtonBase btn, bool value)
        {
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndeterminate(btn, value);
            MaterialDesignThemes.Wpf.ButtonProgressAssist.SetIsIndicatorVisible(btn, value);
        }
    }
}
