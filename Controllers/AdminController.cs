using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Model;
namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {

        private readonly ILogger<AdminController> _logger;
        public Model.DbstorageContext DbContext = new Model.DbstorageContext();

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetUsers")] //вход , регистрация были сделаны до этого
        public IActionResult GetUsers()
        {
            //var Users = DbContext.UsersWithroles.ToList();
            //var data = Users.Select(x => new UserDTO(x)).ToArray();

            return Ok(DbContext.UsersWithroles.ToArray());
        }

        [HttpGet("GetOperations")]//для админа: c складами
        public IActionResult GetOperations()
        {
            return Ok(DbContext.PkqOperationsWithstorages.ToArray());
        }

        //======================================================================


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
