using StorageApp.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Net.Http;
using System.Drawing;
using System.IO;
using QRCoder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace StorageApp
{
    public static class Context
    {
        static string host = "https://localhost:7005";
        static public User UserEnterCheck(string login, string password)
        {
            User user = null;
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

        static public async Task<HttpResponseMessage> postNewPackageAsync(
            decimal weight,
            int unitofWeightId,
            string dimensionId,
            string senderFname,
            string senderSname,
            string senderLname,
            string senderMail,
            string senderNumber,
            string recipientFname,
            string recipientSname,
            string recipientLname,
            string recipientMail,
            string recipientNumber,
            int user_id)
           {
            var parameters = new Dictionary<string, string>
            {
                ["weight"] = weight.ToString(CultureInfo.InvariantCulture),
                ["unitofWeightId"] = unitofWeightId.ToString(),
                ["dimensionId"] = dimensionId,
                ["senderFname"] = senderFname,
                ["senderSname"] = senderSname,
                ["senderLname"] = senderLname,
                ["senderMail"] = senderMail,
                ["senderNumber"] = senderNumber,
                ["recipientFname"] = recipientFname,
                ["recipientSname"] = recipientSname,
                ["recipientLname"] = recipientLname, 
                ["recipientMail"] = recipientMail,
                ["recipientNumber"] = recipientNumber,
                ["user_id"] = user_id.ToString()
            };

            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var param in parameters)
            {
                queryString[param.Key] = param.Value;
            }

            using var httpClient = new HttpClient();
            string requestUrl = $"{host}/Packages/CreatePackage?{queryString}";



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

        static public string TextBeforeSpace(string text)
        {
            return text.Split(' ', 2)[0];
        }


        static public Dictionary<string, string> statusTranslate = new Dictionary<string, string>()
        {
            { "declare", "обьявлен"},
            { "transfer", "в пути на пвз"},
            { "received", "получен на пвз"},
            { "issue", "выдана"},
            { "обьявлен", "обьявлен"},
            { "в пути на пвз", "в пути на пвз"},
            { "получен на пвз", "получен на пвз"},
            { "выдана", "выдана"}
        };

        static public Dictionary<string, string> roleTranslate = new Dictionary<string, string>()
        {
            { "manager", "Администратор"},
            { "storekeeper", "оператор распределительного центра"},
            { "Администратор", "Администратор"},
            { "оператор распределительного центра", "оператор распределительного центра"},
        };

        static public Dictionary<string, string> dimensionsTranslate = new Dictionary<string, string>()
        {
            { "L_box", "Большая Посылка"},
            { "m_box", "Средняя посылка"},
            { "pack", "Пакет"},
            { "s_box", "Маленькая посылка"},
        };

        static public Dictionary<string, string> propertiesTranslate = new Dictionary<string, string>()
{
            //склады
            { "storageid", "Номер склада" },
            { "storageaddr", "Адрес склада" },
    
            //посылки
            { "packageid", "Номер посылки" },
            { "weight", "вес" },
            { "weightunit", "мера веса" },

            { "senderfname", "Имя отправителя" },
            { "sendersname", "Фамилия отправителя" },
            { "senderlname", "Отчество отправителя" },
            { "sendermail", "э.почта отправителя" },
            { "sendernumber", "номер т. отправителя" },

            { "recipientfname", "Имя получателя" },
            { "recipientsname", "Фамилия получателя" },
            { "recipientlname", "Отчество получателя" },
            { "recipientmail", "э.почта получателя" },
            { "recipientnumber", "номер т. получателя" },
            
            { "dimensiontitle", "Классификация обьема" },
            { "unitofweight_title", "Мера веса" },


            { "status", "Статус" },
            { "statusdate", "Дата изменения статуса" },
            { "actionstorageid", "Номер склада исполнителя" },

            //операции
            { "operationid", "номер операции" },
            { "userid", "идентификатор пользователя" },
            { "type", "Тип операции" },
            { "operationdate", "Дата проведения операции" },
            { "typeid", "номер типа" },
            { "commandingstorageid", "Номер склада исполнителя" },

            //пользователи
            { "STORAGEID", "Номер склада" },
            { "roleid", "Номер роли" },
            { "role", "Роль" },
            { "login", "Логин" },
            { "password", "Пароль" },
            { "firstname", "Имя" },
            { "lastname", "Фамилия" },
            { "phonenum", "Номер Телефона" },

        };

        public static void GenerateAndSaveQRCodeAsPdf(string text, string pdfPath)
        {
            // Создаем директорию, если не существует
            Directory.CreateDirectory(Path.GetDirectoryName(pdfPath));

            // Генерируем QR-код
            Bitmap qrImage = null;
            FileStream fileStream = null;
            Document document = null;
            PdfWriter writer = null;
            MemoryStream imageStream = null;

            try
            {
                // 1. Генерация QR-кода
                qrImage = GenerateQRCode(text, 25); // Увеличенный размер для лучшей читаемости

                // 2. Создание PDF документа
                document = new Document();
                fileStream = new FileStream(pdfPath, FileMode.Create);
                writer = PdfWriter.GetInstance(document, fileStream);

                document.Open();

                // 3. Конвертация изображения
                imageStream = new MemoryStream();
                qrImage.Save(imageStream, ImageFormat.Png);
                imageStream.Position = 0; // Сбрасываем позицию потока

                var pdfImage = iTextSharp.text.Image.GetInstance(imageStream.ToArray());

                // 4. Настройка размера и положения
                pdfImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                // Автоматическое масштабирование под страницу
                if (pdfImage.Width > document.PageSize.Width - 72 ||
                    pdfImage.Height > document.PageSize.Height - 72)
                {
                    pdfImage.ScaleToFit(document.PageSize.Width - 72, document.PageSize.Height - 72);
                }

                document.Add(pdfImage);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Debug.WriteLine($"Ошибка при генерации PDF: {ex.Message}");
                throw; // Можно заменить на свой обработчик ошибок
            }
            finally
            {
                // 5. Корректное освобождение ресурсов
                document?.Close();
                writer?.Close();
                fileStream?.Close();
                imageStream?.Close();
                qrImage?.Dispose();
            }
        }


        private static Bitmap GenerateQRCode(string text, int size = 20)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(size, Color.Black, Color.White, true);
        }

    }
}
