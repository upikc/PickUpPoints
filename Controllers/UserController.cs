using Microsoft.AspNetCore.Mvc;
using StorageApi.Model;
namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<PackagesController> _logger;
        public Model.DbstorageContext DbContext = new Model.DbstorageContext();


        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            //var Users = DbContext.UsersWithroles.ToList();
            //var data = Users.Select(x => new UserDTO(x)).ToArray();

            return Ok(DbContext.UsersWithroles.ToArray());
        }

        /// <summary>
        /// Получает пользователя по его ID.
        /// </summary>
        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            var user = DbContext.UsersWithroles.FirstOrDefault(x=>x.UserId == id);

            if (user == default)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Добавляет нового пользователя.
        /// </summary>
        [HttpPost("CreateNewUser")]
        public IActionResult CreateNewUser([FromBody] Model.User newUser)
        {
            try
            {
                DbContext.Users.Add(newUser);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
            return Ok();
        }


        /// <summary>
        /// Проверка данных нового пользователя
        /// </summary>
        [HttpGet("DataValidCheck")]
        public IActionResult ValidCheck(string login, string password, string fName , string Lname, string phoneNumber, int roleID, int storageId)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fName)
                || string.IsNullOrWhiteSpace(Lname) || string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest("Поля пусты");
            if (!DbContext.UserRoles.Any(x => x.RoleId == roleID)) //если нет роли
                return BadRequest("Роль не выбрана");
            if (!DbContext.Storages.Any(x => x.StorageId == storageId)) //если нет склада
                return BadRequest("Склад не существует");

            return Ok("Поля валидны");
        }

        /// <summary>
        /// Аутентификация 
        /// </summary>
        [HttpGet("EnterValidCheck")]
        public IActionResult ValidCheck(string login, string password)
        {
            var user = DbContext.UsersWithroles.FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user == default)
            {
                return StatusCode(406);
            }
            else
                return Ok(user);
        }
    }
}
