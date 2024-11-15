using StorageApp.Model;
using System.Net.Http;
using System.Net.Http.Json;

namespace StorageApp
{
    public class DataContext
    {
        static string host = "https://localhost:7005";
        public User UserEnterCheck(string login, string password)
        {
            User user = default;
            try
            {
                using var httpclient = new HttpClient();
                string requestUri = $"{host}/User/EnterValidCheck?login={login}&password={password}";
                user = httpclient.GetFromJsonAsync<User>(requestUri).Result;
            }
            catch (Exception ex){}
            return user;
        }

        public Package[] getPackages()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/Packages/GetPackages";
            Package[] packages = httpclient.GetFromJsonAsync<Package[]>(requestUri).Result;

            return packages;
        }

        public Operation[] getOperations()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/TransferOperation/GetOperations";
            Operation[] operation = httpclient.GetFromJsonAsync<Operation[]>(requestUri).Result;

            return operation;
        }
    }
}
