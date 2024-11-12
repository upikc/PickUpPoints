using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Model;
using System.Linq;
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

    }
}
