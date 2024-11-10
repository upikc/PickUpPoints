using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetPackages")]
        public IActionResult GetPackages()
        {
            return Ok(DbContext.PackagesWithstatuses.ToArray());
        }

        [HttpGet("GetOperations")]//для админа: c складами
        public IActionResult GetOperations()
        {
            return Ok(DbContext.PkqOperationsWithstorages.ToArray());
        }
    }
}
