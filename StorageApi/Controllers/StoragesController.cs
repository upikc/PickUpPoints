using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Model;
using System.Linq;
namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoragesController : ControllerBase
    {

        private readonly ILogger<StoragesController> _logger;
        public Model.DbstorageContext DbContext = new Model.DbstorageContext();

        public StoragesController(ILogger<StoragesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получает все склады 
        /// </summary>
        [HttpGet("GetStorages")]
        public IActionResult GetStorages()
        {
            return Ok(DbContext.Storages.Select(x => new Model.StorageDTO(x)).ToArray());
        }


        /// <summary>
        /// Добавляет нового склада.
        /// </summary>
        [HttpPost("CreateNewStorages")]
        public IActionResult CreateNewStorages([FromBody] string adress)
        {
            try
            {
                var storage = new Storage();
                storage.StorageId = DbContext.Storages.Max(x => x.StorageId) + 1;
                storage.StorageAddr = adress;
                storage.Enable = 1;
                DbContext.Storages.Add(storage);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Изминение данных склада.
        /// </summary>
        [HttpPost("ChangeStorageData")]
        public IActionResult CreateNewStorages([FromBody] string adress, int Enable, int storageId)
        {
            try
            {
                var storage = DbContext.Storages.First(x => x.StorageId == storageId);

                if (adress != null)
                storage.StorageAddr = adress;

                
                storage.Enable = Enable;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok();
        }
    }
}
