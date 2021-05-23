using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.ServerActions
{
    /// <summary>
    /// Класс создан для возможного расширения метода SendRequest в RequestsHelper
    /// </summary>
    class RequestHeader
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
