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
        /// Получает операции определенного заказа 
        /// </summary>
        [HttpGet("GetPkgOperations")] //Операции содержат статусы
        public IActionResult GetPkgOperations(int pkg_id)
        {
            if (!DbContext.Packages.Any(x => x.PackageId == pkg_id))
                return StatusCode(406);

            try
            {
                var data = DbContext.PkqOperationsWithstorages.Where(x => x.PackageId == pkg_id).ToArray();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok(DbContext.PkqOperationsWithstorages.Where(x => x.PackageId == pkg_id).ToArray());
        }

        /// <summary>
        /// Создает новый заказ 
        /// </summary>
        [HttpPost("CreatePackage")]
        public IActionResult CreatePackage([FromBody] Package Pack)
        {
            var Packid = DbContext.Packages.Max(p => p.PackageId) + 1;
            Pack.PackageId = Packid;
            DbContext.Packages.Add(Pack);
            try
            {
                PkgOperation pkgOP = new PkgOperation();

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
