using StorageApp.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;

namespace StorageApp
{
    public static class Context
    {
        static string host = "https://localhost:7005";
        static public User UserEnterCheck(string login, string password)
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
        static public Storage[] getStorages()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/Storages/GetStorages";
            Storage[] storages = httpclient.GetFromJsonAsync<Storage[]>(requestUri).Result;

            return storages;
        }

        static public User[] getUsers()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/User/GetUsers";
            User[] Users = httpclient.GetFromJsonAsync<User[]>(requestUri).Result;

            return Users;
        }

        static public Package[] getPackages()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/Packages/GetPackages";
            Package[] packages = httpclient.GetFromJsonAsync<Package[]>(requestUri).Result;

            return packages;
        }

        static public Package[] getPackagesFromStorage(int storageID)
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/Packages/GetPackagesByStorageID?storageId={storageID}";
            Package[] packagesFromStorage = httpclient.GetFromJsonAsync<Package[]>(requestUri).Result;

            return packagesFromStorage;
        }

        static public Operation[] getOperations()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/TransferOperation/GetOperations";
            Operation[] operation = httpclient.GetFromJsonAsync<Operation[]>(requestUri).Result;

            return operation;
        }

        static public async Task<string> postNewStorageAsync(string adress) //по этому примеру переделать все 
        {

            try
            {
                using var httpClient = new HttpClient();

                var response = await httpClient.PostAsJsonAsync($"{host}/Storages/CreateNewStorages", adress);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return("Склад успешно создан: " + responseContent);
                }
                else
                {
                    return ("Ошибка: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
                return ("Ошибка HTTP-запроса: " + ex.Message);
            }
            catch (Exception ex)
            {
                return ("Непредвиденная ошибка: " + ex.Message);
            }
        }


        static public Task<HttpResponseMessage> checkUserDataValid(string login , string password , string fName , string lName , string PhonNumb , int RoleId , int Storage)
        {
            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/User/DataValidCheck?login={login}&password={password}&fName={fName}" +
                                $"&Lname={lName}&phoneNumber={PhonNumb}&roleID={RoleId}&storageId={Storage}";

            HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

            return Task.FromResult(response);
            
        }
        static public async Task<HttpResponseMessage> postNewUserAsync(string login, string password, string fName, string lName, string phoneNumb, int roleId, int storageId)
        {

            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/User/CreateNewUser?login={login}&password={password}&fName={fName}&Lname={lName}&phoneNumber={phoneNumb}&roleID={roleId}&storageId={storageId}";

            var response = await httpClient.PostAsync(requestUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error response: " + responseBody);
            }
            return response;

        }

        static public async Task<HttpResponseMessage> postNewPackageAsync(decimal weight, string ClientFullName, string Mail, string Number, int AdminId)
        {

            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/Packages/CreatePackage?weight={weight}&ClientFullName={ClientFullName}&Mail={Mail}&Number={Number}&AdminId={AdminId}";

            var response = await httpClient.PostAsync(requestUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error response: " + responseBody);
            }
            return response;

        }

        static public async Task<HttpResponseMessage> postNewPkgOperationAsync(int pkgId, int userId, int TypeOfOperation, int ActionStorageID)
        {

            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/TransferOperation/CreateTransferOperation?pkgId={pkgId}&userId={userId}&TypeOfOperation={TypeOfOperation}&ActionStorageID={ActionStorageID}";

            var response = await httpClient.PostAsync(requestUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error response: " + responseBody);
            }
            return response;
        }

        static public bool ContainsNullOrWhiteSpace(string[] array)
        {
            if (array == null)
            {
                return true;
            }

            foreach (var str in array)
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return true;
                }
            }

            return false;
        }
        static public int NumbBeforeSpace(string text)
        {
            return int.Parse(text.Split(' ', 2)[0]);
        }

    }
}
