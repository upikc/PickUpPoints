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
        /// Создает новую операцию 
        /// </summary>
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
                pkgOP.TypeId = 1;
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
