using Microsoft.AspNetCore.Mvc;
using StorageApi.Model;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System;
namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackagesController : ControllerBase
    {

        private readonly ILogger<PackagesController> _logger;
        public Model.DbstorageContext DbContext = new Model.DbstorageContext();

        public PackagesController(ILogger<PackagesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получает все заказы 
        /// </summary>
        [HttpGet("GetPackages")]
        public IActionResult GetPackages()
        {
            return Ok(DbContext.PackagesWithstatuses.ToArray());
        }

        /// <summary>
        /// Получает сообщения для трекинга 
        /// </summary>
        [HttpGet("GetPackageTracking")]
        public IActionResult GetPackageTracking(int packageId)
        {

            try
            {
                // Получаем основную информацию о посылке
                var package = DbContext.PackagesWithstatuses
                    .FirstOrDefault(p => p.PackageId == packageId);

                if (package == null)
                {
                    _logger.LogWarning("Посылка {PackageId} не найдена", packageId);
                    return NotFound($"Посылка {packageId} не найдена");
                }

                // Получаем историю операций
                var operations = DbContext.PkqOperationsWithstorages
                    .Where(o => o.PackageId == packageId)
                    .OrderBy(o => o.OperationDate)
                    .ToList();

                // Генерируем сообщение
                var trackingMessage = Metods.GetTrackerMessage(package, operations);


                return Ok(trackingMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении трекинг-информации для посылки {PackageId}", packageId);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

    

        /// <summary>
        /// Получает все заказы определенного склада
        /// </summary>
        [HttpGet("GetPackagesByStorageID")]
        public IActionResult GetPackagesByStorageID(int storageId)
        {
            return Ok(DbContext.PackagesWithstatuses.Where(x => x.ActionstorageId == storageId ).ToArray());
        }

        /// <summary>
        /// Создает новый заказ 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        /// POST /Todo
        /// {
        ///     "packageId": 0,
        ///     "weight": 0,
        ///     "clientFullname": "string",
        ///     "clientMail": "string",
        ///     "clientNumber": "string"
        /// }
        /// </remarks>
        [HttpPost("CreatePackage")]
        public IActionResult CreatePackage(
            decimal weight,
            int unitofWeightId,
            string dimensionId,
            string senderFname,
            string senderSname,
            string senderLname,
            string senderMail,
            string? senderNumber,
            string recipientFname,
            string recipientSname,
            string recipientLname,
            string recipientMail,
            string? recipientNumber,
            int user_id)
        {

            var Packid = DbContext.Packages.Count() + 1;

            if (!DbContext.UnitofWeights.Any(u => u.UnitofWeightId == unitofWeightId))
                return BadRequest("Недопустимая единица измерения веса");

            var user = DbContext.Users.FirstOrDefault(u => u.UserId == user_id && u.RoleId == 2);
            if (user == null)
                return BadRequest("Недостаточно прав для создания посылки");



            var Pack = new Package
            {
                PackageId = Packid,
                Weight = weight,
                UnitofWeightId = unitofWeightId,
                DimensionId = dimensionId,
                SenderFname = senderFname,
                SenderSname = senderSname,
                SenderLname = senderLname,
                SenderMail = senderMail,
                SenderNumber = senderNumber,
                RecipientFname = recipientFname,
                RecipientSname = recipientSname,
                RecipientLname = recipientLname,
                RecipientMail = recipientMail,
                RecipientNumber = recipientNumber
            };

            var initialOperation = new PkgOperation
            {
                Package = Pack,
                UserId = user_id,
                TypeId = 0, //
                            // операции "Создание"
                OperationDate = DateTime.Now,
                ActionstorageId = user.StorageId
            };


            DbContext.Packages.Add(Pack);
            try
            {

                DbContext.Packages.Add(Pack);
                DbContext.PkgOperations.Add(initialOperation);
                DbContext.SaveChanges();
                // ТУТБУДЕТ СМС С ОПОВЕЩЕНИЕМ ОП ПОСЫЛКЕ

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok("Сохранено успешно");
        }


        /// <summary>
        /// Получение кода для проверки 
        /// </summary>
        [HttpGet("getPackCode")]
        public IActionResult getPackCode(int PackId)
        {
            return Ok(Metods.GetLast4CharHashFromString(PackId));
        }


    }
    public static class Metods
    {
        public static void SendPackageMail(PackagesWithstatus package)
        {
            if (package == default)
                return;

            string mail = package.RecipientMail;



            SmtpClient smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("artemyusup@yandex.ru", "ZOID"),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("artemyusup@yandex.ru"),
                Subject = "Код для получения заказа",
                Body = $@"
                <div style='font-size: 20px;'>
                    Ваш код: 
                    <span style='font-size: 24px; font-weight: bold;'>
                        {GetLast4CharHashFromString(package.PackageId)}
                    </span>
                </div>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(mail);
            smtpClient.Send(mailMessage);

        }

        public static string GetLast4CharHashFromString(int numb)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(numb.ToString()));
                return (BitConverter.ToString(hashBytes).Replace("-", "").ToLower())[^4..];
            }
        }


        public static string GetTrackerMessage(PackagesWithstatus package, List<PkqOperationsWithstorage> operations)
        {
            var sb = new StringBuilder();

            // Заголовок
            sb.AppendLine($"📦 Трекинг посылки #**{package.PackageId}**");
            sb.AppendLine("----------------------------------------");

            // Основная информация о посылке
            sb.AppendLine("\n📄 **Основные данные:**");
            sb.AppendLine($"⚖️ Вес: {package.Weight} {package.WeightUnit ?? ""}");
            sb.AppendLine($"📏 Тип упаковки: {package.DimensionTitle ?? "не указан"}");
            sb.AppendLine($"📅 Дата последнего статуса: {package.StatusDate:dd.MM.yyyy HH:mm}");
            sb.AppendLine("----------------------------------------");

            // Информация об отправителе
            sb.AppendLine("\n📤 **Отправитель:**");
            sb.AppendLine($"👤 {package.SenderLname} {package.SenderFname} {package.SenderSname}");
            sb.AppendLine($"📧 {package.SenderMail}");
            sb.AppendLine($"📞 {package.SenderNumber ?? "не указан"}");

            // Информация о получателе
            sb.AppendLine("\n📥 **Получатель:**");
            sb.AppendLine($"👤 {package.RecipientLname} {package.RecipientFname} {package.RecipientSname}");
            sb.AppendLine($"📧 {package.RecipientMail}");
            sb.AppendLine($"📞 {package.RecipientNumber ?? "не указан"}");
            sb.AppendLine("----------------------------------------");

            // История операций
            sb.AppendLine("\n🕒 **История перемещений:**");
            if (operations.Any())
            {
                foreach (var op in operations.OrderBy(o => o.OperationDate))
                {
                    var emoji = op.Type switch
                    {
                        "declare" => "📝",
                        "transfer" => "🚚",
                        "received" => "📦",
                        "issue" => "✅",
                        _ => "🔹"
                    };

                    sb.AppendLine($"\n{emoji} **{GetRussianStatus(op.Type)}**");
                    sb.AppendLine($"🗓️ {op.OperationDate:dd.MM.yyyy HH:mm}");
                    sb.AppendLine($"🏢 Склад: {op.ActionstorageId}");
                }
            }
            else
            {
                sb.AppendLine("\nИстория перемещений отсутствует");
            }

            // Текущий статус
            sb.AppendLine("\n----------------------------------------");
            sb.AppendLine($"🚩 **Текущий статус:** {GetRussianStatus(package.Status)}");
            sb.AppendLine("\nℹ️ Для уточнения деталей обращайтесь в поддержку");

            return sb.ToString();
        }

        private static string GetRussianStatus(string englishStatus)
        {
            return englishStatus.ToLower() switch
            {
                "declare" => "Оформлена",
                "transfer" => "В пути",
                "received" => "Принята на складе",
                "issue" => "Выдана",
                _ => englishStatus
            };
        }


    }

}
