using Microsoft.AspNetCore.Mvc;
using StorageApi.Model;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult CreatePackage(decimal weight , string ClientFullName , string Mail , string Number , int AdminId)
        {
            var Packid = DbContext.Packages.Max(p => p.PackageId) + 1;
            Package Pack = new Package(0 , weight , ClientFullName , Mail , Number);
            Pack.PackageId = Packid;
            DbContext.Packages.Add(Pack);
            try
            {
                PkgOperation pkgOP = new PkgOperation(); //посылка создана, создаем операцию того как она была добавлена

                if (!DbContext.Users.Any(u => u.UserId == AdminId && u.RoleId == 1)) //в идеале для ручных запросов
                    return BadRequest("Запрос на добавление от неизвестного администратора");

                //pkg.OperationId = AI
                pkgOP.PackageId = Packid;
                pkgOP.UserId = AdminId;
                pkgOP.TypeId = 0;
                pkgOP.OperationDate = DateTime.Now;
                pkgOP.ActionstorageId = DbContext.Users.First(x => x.UserId == AdminId).StorageId;
                DbContext.PkgOperations.Add(pkgOP);

                DbContext.SaveChanges();

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

            string mail = package.ClientMail;



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
    }

}
