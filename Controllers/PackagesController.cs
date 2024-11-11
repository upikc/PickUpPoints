using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("GetPackages")] //Операции содержат статусы
        public IActionResult GetPackages()
        {
            return Ok(DbContext.PackagesWithstatuses.ToArray());
        }

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
    }
}
