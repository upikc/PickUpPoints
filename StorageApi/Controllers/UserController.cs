﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Model;
using System.Text.RegularExpressions;
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
        public IActionResult CreateNewUser(string login, string password, string fName, string Lname, string phoneNumber, int roleID, int storageId)
        {
            try
            {
                var newUser = new User(login , password , fName , Lname , phoneNumber , roleID , storageId);
                newUser.UserId = DbContext.Users.Max(x => x.UserId) + 1;
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
        /// Изменение пароля пользователя.
        /// </summary>
        [HttpPost("UserPasswordChange")]
        public IActionResult UserPasswordChange(int userId , string newPass)
        {
            try
            { 
                DbContext.Users.First(x => x.UserId == userId).Password = newPass;
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

            var phonePattern = "^(\\+7|8)[\\s\\-]?\\(?\\d{3}\\)?[\\s\\-]?\\d{3}[\\s\\-]?\\d{2}[\\s\\-]?\\d{2}$";
            if (!Regex.IsMatch(phoneNumber, phonePattern))
                return BadRequest("Номер телефона не валиден");

            if (DbContext.UsersWithroles.Any(x=> x.Login == login))
                return BadRequest("Данный логин уже занят");
            if (DbContext.UsersWithroles.Any(x => x.PhoneNum == phoneNumber))
                return BadRequest("Номер телефона занят");
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
