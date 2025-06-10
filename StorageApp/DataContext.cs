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
using System.Drawing;
using System.IO;
using QRCoder;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Drawing.Imaging;
using System;
using BarcodeStandard;
using SkiaSharp;

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
            catch (Exception ex) { }
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
                    return ("Склад успешно создан: " + responseContent);
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


        static public Task<HttpResponseMessage> checkUserDataValid(string login, string password, string fName, string lName, string PhonNumb, int RoleId, int Storage)
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
            int user_id,
            int destinationStorageId) 
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
                ["user_id"] = user_id.ToString(),
                ["destinationStorageId"] = destinationStorageId.ToString()
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
            { "manager", "администратор"},
            { "storekeeper", "оператор распределительного центра"},
            { "администратор", "администратор"},
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
            { "StatusText", "Статус"},

        };

        static public Dictionary<string, string> EnDisTranslater = new Dictionary<string, string>()
        {
            { "1", "Активен"},
            { "0", "Отключен"}
        };

        public static void GenerateAndSaveBarcodeAsPdf(string text, string pdfPath, string message)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            if (!File.Exists(fontPath))
            {
                string localFontFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts");
                Directory.CreateDirectory(localFontFolder); // Ensure the directory exists
                string localArialPath = Path.Combine(localFontFolder, "arial.ttf");

                if (File.Exists(localArialPath))
                {
                    fontPath = localArialPath;
                }
                else
                {
                    Console.WriteLine($"Warning: arial.ttf not found at '{fontPath}' or '{localArialPath}'. " +
                                      "Cyrillic text might not display correctly without a suitable font.");
                    fontPath = null;
                }
            }

            BaseFont baseFont;
            iTextSharp.text.Font russianFont;

            try
            {
                if (fontPath != null)
                {
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else
                {
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                    System.Diagnostics.Debug.WriteLine("Fallback to HELVETICA with CP1250 due to missing Arial. Cyrillic support might be limited.");
                }
                russianFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating font: {ex.Message}. Falling back to default.");
                russianFont = FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(pdfPath));

            Bitmap barcodeImage = null;
            FileStream fileStream = null;
            Document document = null;
            PdfWriter writer = null;
            MemoryStream imageStream = null;

            try
            {
                barcodeImage = GenerateBarcode(text);
                document = new Document();
                fileStream = new FileStream(pdfPath, FileMode.Create);
                writer = PdfWriter.GetInstance(document, fileStream);

                document.Open();

                imageStream = new MemoryStream();
                barcodeImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                imageStream.Position = 0;

                var pdfImage = iTextSharp.text.Image.GetInstance(imageStream.ToArray());
                pdfImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                if (pdfImage.Width > document.PageSize.Width - 72 ||
                    pdfImage.Height > document.PageSize.Height - 72)
                {
                    pdfImage.ScaleToFit(document.PageSize.Width - 72, document.PageSize.Height - 72);
                }

                document.Add(pdfImage);

                if (!string.IsNullOrEmpty(message))
                {
                    document.Add(new iTextSharp.text.Paragraph("\n"));
                    var paragraph = new iTextSharp.text.Paragraph(message, russianFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(paragraph);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generating PDF: {ex.Message}");
                throw; 
            }
            finally
            {
                document?.Close();
                writer?.Close();
                fileStream?.Close();
                imageStream?.Close();
                barcodeImage?.Dispose();
            }

            try
            {
                Process.Start(new ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error opening PDF: {ex.Message}");
            }
        }

        private static Bitmap GenerateBarcode(string text)
        {
            SKColorF blackColor = new SKColorF(Color.Black.R / 255f, Color.Black.G / 255f, Color.Black.B / 255f, Color.Black.A / 255f);
            SKColorF whiteColor = new SKColorF(Color.White.R / 255f, Color.White.G / 255f, Color.White.B / 255f, Color.White.A / 255f);
            var barcode = new BarcodeStandard.Barcode();
            var skBitmap = barcode.Encode(BarcodeStandard.Type.Code128, text, blackColor, whiteColor, 290, 120);

            // Конвертация SKBitmap в System.Drawing.Bitmap
            var bitmap = new Bitmap(skBitmap.Width, skBitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                            System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                            bitmap.PixelFormat);

            skBitmap.ReadPixels(new SKImageInfo(bitmapData.Width, bitmapData.Height),
                               bitmapData.Scan0,
                               bitmapData.Stride,
                               0, 0);

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        //              ТУТ функционал вычисления стоимости

        static Dictionary<string, int> sizeCost = new Dictionary<string, int>
        {
            { "Стандарт коробка до 530×360×220 мм.", 500 },
            { "Стандарт коробка до 400×270×180 мм.", 400 },
            { "Мелкий пакет", 200 },
            { "Стандарт коробка до 260×170×80 мм", 300 },
            { "L_box", 500 },
            { "m_box", 400 },
            { "pack", 200 },
            { "s_box", 300 }
        };

        static Dictionary<string, int> weightUnitCost = new Dictionary<string, int>
        {
            { "Грамм", 4 },    // стоимость за грамм (меньше = дороже)
            { "Килограмм", 800 }  // стоимость за килограмм
        };

        static int CalculatePackPrise(string size, string weightUnit, int weight)
        {
            int sizeCostValue = sizeCost[size];
            int weightUnitCostValue = weightUnitCost[weightUnit];

            int totalCost = sizeCostValue + (weight * weightUnitCostValue);

            return totalCost;
        }

        public static void GenerateAndSaveReceiptAsPdf(Package pack, string pdfPath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            if (!File.Exists(fontPath))
            {
                string localFontFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fonts");
                Directory.CreateDirectory(localFontFolder);
                string localArialPath = Path.Combine(localFontFolder, "arial.ttf");

                if (File.Exists(localArialPath))
                {
                    fontPath = localArialPath;
                }
                else
                {
                    Console.WriteLine($"Warning: arial.ttf not found at '{fontPath}' or '{localArialPath}'. " +
                                      "Cyrillic text might not display correctly without a suitable font.");
                    fontPath = null;
                }
            }

            BaseFont baseFont;
            iTextSharp.text.Font russianFont;

            try
            {
                if (fontPath != null)
                {
                    baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else
                {
                    baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
                    System.Diagnostics.Debug.WriteLine("Fallback to HELVETICA with CP1250 due to missing Arial. Cyrillic support might be limited.");
                }
                russianFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating font: {ex.Message}. Falling back to default.");
                russianFont = FontFactory.GetFont(FontFactory.HELVETICA, BaseFont.CP1252, BaseFont.EMBEDDED);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(pdfPath));

            Document document = null;
            PdfWriter writer = null;
            FileStream fileStream = null;

            try
            {
                document = new Document();
                fileStream = new FileStream(pdfPath, FileMode.Create);
                writer = PdfWriter.GetInstance(document, fileStream);

                document.Open();

                // Generate barcode image
                Bitmap barcodeImage = GenerateBarcode(pack.PackageId.ToString());
                MemoryStream imageStream = new MemoryStream();
                barcodeImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
                imageStream.Position = 0;

                var pdfImage = iTextSharp.text.Image.GetInstance(imageStream.ToArray());
                pdfImage.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                if (pdfImage.Width > document.PageSize.Width - 72 ||
                    pdfImage.Height > document.PageSize.Height - 72)
                {
                    pdfImage.ScaleToFit(document.PageSize.Width - 72, document.PageSize.Height - 72);
                }

                document.Add(pdfImage);

                // Add package details
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph($"Отправитель: {pack.senderFullName()}", russianFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph($"Получатель: {pack.recipientFullName()}", russianFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph($"Вес: {pack.Weight} {pack.WeightUnit}", russianFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph($"Размер: {pack.DimensionTitle}", russianFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph($"Дата обьявления: {Context.getOperations().First(x => x.PackageId == pack.PackageId).OperationDate}", russianFont) { Alignment = Element.ALIGN_LEFT });

                // Calculate and add shipping cost
                int shippingCost = CalculatePackPrise(pack.DimensionTitle, pack.WeightUnit , (int)pack.Weight);
                document.Add(new Paragraph($"Shipping Cost: {shippingCost}", russianFont) { Alignment = Element.ALIGN_LEFT });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error generating PDF: {ex.Message}");
                throw;
            }
            finally
            {
                document?.Close();
                writer?.Close();
                fileStream?.Close();
            }

            try
            {
                Process.Start(new ProcessStartInfo(pdfPath)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error opening PDF: {ex.Message}");
            }
        }

    }
}
