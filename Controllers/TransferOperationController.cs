using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Model;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferOperationController : ControllerBase
    {

        private readonly ILogger<TransferOperationController> _logger;
        public Model.DbstorageContext DbContext = new Model.DbstorageContext();

        public TransferOperationController(ILogger<TransferOperationController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получает все операции 
        /// </summary>
        [HttpGet("GetOperations")]
        public IActionResult GetOperations()
        {
            return Ok(DbContext.PkqOperationsWithstorages.ToArray());
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
        /// Создает новую операцию 
        /// </summary>
        /// /// <remarks>
        /// Пример запроса:
        ///
        /// POST /Todo
        /// {
        ///     Дописать СЮДА
        /// }
        /// </remarks>
        [HttpPost("CreateTransferOperation")]
        public IActionResult CreateTransferOperation([FromBody] int pkgId, int userId, int storageID)
        {
            var package = DbContext.PackagesWithstatuses.FirstOrDefault(x => x.PackageId == pkgId);
            if (package == default || package.Status != "declare" || !DbContext.Storages.Any(x => x.StorageId == storageID)
                || !DbContext.UsersWithroles.Any(x => x.UserId == userId && x.RoleId == 1))
                return StatusCode(406);

            try
            {
                PkgOperation pkgOP = new PkgOperation();
                //pkg.OperationId = AI
                pkgOP.PackageId = pkgId;
                pkgOP.UserId = userId;
                pkgOP.TypeId = 1; //оператор пвз совершает остальные операии
                pkgOP.OperationDate = DateTime.Now;
                pkgOP.ActionstorageId = storageID;
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
