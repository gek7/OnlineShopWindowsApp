using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;

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

        public static string GetXaml(RichTextBox rt)
        {
            TextRange range = new TextRange(rt.Document.ContentStart, rt.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml);
            string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            return xamlText;
        }

        public static void SetXaml(RichTextBox rt, string xamlString)
        {
            if (!String.IsNullOrEmpty(xamlString))
            {
                StringReader stringReader = new StringReader(xamlString);
                XmlReader xmlReader = XmlReader.Create(stringReader);
                Section sec = XamlReader.Load(xmlReader) as Section;
                FlowDocument doc = new FlowDocument();
                while (sec.Blocks.Count > 0)
                    doc.Blocks.Add(sec.Blocks.FirstBlock);
                rt.Document = doc;
            }
        }

        public static bool isAuth() => MainWindow.User != null && !string.IsNullOrEmpty(MainWindow.User.token);
    }
}
