using Microsoft.AspNetCore.Mvc;
using StorageApi.Model;
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

                //pkg.OperationId = AI
                pkgOP.PackageId = Packid;
                pkgOP.UserId = 0;
                pkgOP.TypeId = 0;
                pkgOP.OperationDate = DateTime.Now;
                pkgOP.ActionstorageId = 0;
                DbContext.PkgOperations.Add(pkgOP);

                DbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok("Сохранено успешно");
        }

    }
}
