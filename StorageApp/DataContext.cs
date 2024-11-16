﻿using StorageApp.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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
        public Storage[] getStorages()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/Storages/GetStorages";
            Storage[] storages = httpclient.GetFromJsonAsync<Storage[]>(requestUri).Result;

            return storages;
        }

        public User[] getUsers()
        {
            using var httpclient = new HttpClient();
            string requestUri = $"{host}/User/GetUsers";
            User[] Users = httpclient.GetFromJsonAsync<User[]>(requestUri).Result;

            return Users;
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

        public async Task<string> postNewStorageAsync(string adress)
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


        public Task<HttpResponseMessage> checkUserDataValid(string login , string password , string fName , string lName , string PhonNumb , int RoleId , int Storage)
        {
            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/User/DataValidCheck?login={login}&password={password}&fName={fName}" +
                                $"&Lname={lName}&phoneNumber={PhonNumb}&roleID={RoleId}&storageId={Storage}";

            HttpResponseMessage response = httpClient.GetAsync(requestUrl).Result;

            return Task.FromResult(response);
            
        }
        public async Task<HttpResponseMessage> postNewUserAsync(string login, string password, string fName, string lName, string phoneNumb, int roleId, int storageId)
        {

            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/User/CreateNewUser?login={login}&password={password}&fName={fName}&Lname={lName}&phoneNumber={phoneNumb}&roleID={roleId}&storageId={storageId}";

            var response = await httpClient.PostAsync(requestUrl, null);
            if (!response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error response: " + responseBody);
            }
            response.EnsureSuccessStatusCode();
            return response;

        }
    }
}
