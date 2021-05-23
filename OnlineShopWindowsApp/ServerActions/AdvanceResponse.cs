using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp
{
    public class AdvanceResponse<T> where T : class
    {
        public AdvanceResponse(HttpResponseMessage sourceResponse, T obj)
        {
            SourceResponse = sourceResponse;
            Obj = obj;
        }

        public HttpResponseMessage SourceResponse { get; }
        public T Obj { get; }
    }
}
