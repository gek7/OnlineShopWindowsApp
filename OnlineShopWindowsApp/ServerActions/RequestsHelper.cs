using OnlineShopWindowsApp.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineShopWindowsApp.ServerActions
{
    class RequestsHelper
    {
        private static HttpClient Client = new HttpClient();
        //Универсальный метод для отправления запросов на сервер
        private async static Task<AdvanceResponse<T>> SendRequest<T>(string url, HttpMethod type, bool isAuthHeader = true, string data = "") where T : class
        {
            data = data == null ? "" : data;
            HttpResponseMessage Resp = null;
            T Obj = null;
            var request = new HttpRequestMessage(type, url);

            if (isAuthHeader) addAuthHeader(request);

            if (type == HttpMethod.Post || type == HttpMethod.Put)
            {
                request.Content = new StringContent(data);
                request.Content.Headers.ContentType.MediaType = "application/json";
//                request.Content.Headers.Add("Content-Type", "application/json");
            }
            Resp = await Client.SendAsync(request);
            if (Resp.IsSuccessStatusCode)
            {
                string jsonObj = await Resp.Content.ReadAsStringAsync();
                Obj = (T)JsonSerializer.Deserialize(jsonObj, typeof(T));
            }
            if (Resp.StatusCode == System.Net.HttpStatusCode.NotFound ||
                Resp.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                MainWindow.mainWindow.mainFrame.Navigate(new ErrorPage(Resp.ReasonPhrase));
            }

            return new AdvanceResponse<T>(Resp, Obj);
        }
        public async static Task<AdvanceResponse<T>> PostRequest<T>(string url, T data = null, bool isAuthHeader = true) where T : class
        {
            string json = null;
            if (data != null) json = JsonSerializer.Serialize(data, typeof(T));
            return await SendRequest<T>(url, HttpMethod.Post, isAuthHeader, json);
        }
        public async static Task<AdvanceResponse<T>> PutRequest<T>(string url, T data = null, bool isAuthHeader = true) where T : class
        {
            string json = null;
            if (data != null) json = JsonSerializer.Serialize(data, typeof(T));
            return await SendRequest<T>(url, HttpMethod.Put, isAuthHeader, json);
        }
        public async static Task<AdvanceResponse<T>> DeleteRequest<T>(string url, bool isAuthHeader = true, string data = "") where T : class
        {
            return await SendRequest<T>(url, HttpMethod.Delete, isAuthHeader, data);
        }
        public async static Task<AdvanceResponse<T>> GetRequest<T>(string url, bool isAuthHeader = true) where T : class
        {
            return await SendRequest<T>(url, HttpMethod.Get, isAuthHeader);
        }
        public async static Task<HttpResponseMessage> SendFile(string url, string fileName, bool isAuthHeader = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (isAuthHeader) addAuthHeader(request);
            MultipartFormDataContent form = new MultipartFormDataContent();
            var bytes = File.ReadAllBytes(fileName);
            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", Path.GetFileName(fileName));
            request.Content = form;
            HttpResponseMessage response = await Client.SendAsync(request);
            return response;
        }
        private static void addAuthHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Authorization", "Bearer " + MainWindow.User.token);
        }
    }
}
