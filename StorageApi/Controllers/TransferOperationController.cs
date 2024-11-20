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
        public IActionResult CreateTransferOperation(int pkgId, int userId, int TypeOfOperation , int ActionStorageID)
        {

            var package = DbContext.PackagesWithstatuses.FirstOrDefault(x => x.PackageId == pkgId);
            var User = DbContext.UsersWithroles.FirstOrDefault(x => x.UserId == userId);

            if (package == default || User == default || !new[] { 1, 2, 3, 4 }.Contains(TypeOfOperation))
                return BadRequest("поля не верны");

            var UserRole = User.RoleId;

            //0-declare   1-manager
            //1-transfer  

            //2-received  2-storekeeper
            //3-issue


            var user = DbContext.Users.FirstOrDefault(x => x.UserId == userId);
            if (package == default || !DbContext.Storages.Any(x => x.StorageId == user.StorageId)
                || !DbContext.UsersWithroles.Any(x => x.UserId == userId ))
                return StatusCode(406);





            if ((package.Status == "declare" && UserRole == 1 && TypeOfOperation == 1) ||
                (package.Status == "transfer" && UserRole == 2 && TypeOfOperation == 2) ||
                (package.Status == "received" && UserRole == 2 && TypeOfOperation == 3) )
            try
            {
                PkgOperation pkgOP = new PkgOperation();
                //pkg.OperationId = AI
                pkgOP.PackageId = pkgId;
                pkgOP.UserId = userId;
                pkgOP.TypeId = TypeOfOperation; //оператор пвз совершает остальные операии
                pkgOP.OperationDate = DateTime.Now;
                pkgOP.ActionstorageId = ActionStorageID;
                DbContext.PkgOperations.Add(pkgOP);

                DbContext.SaveChanges();
                return Ok("Сохранено успешно");
                }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }

            return BadRequest("Не верный порядок операций");

        }

    }
}
