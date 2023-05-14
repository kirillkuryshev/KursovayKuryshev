using BLL.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<DAL.Entity.User> _userManager;
        private readonly SignInManager<DAL.Entity.User> _signInManager;
        ILogger logger; // логгер

        public AccountController(UserManager<DAL.Entity.User> userManager,
        SignInManager<DAL.Entity.User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<AccountController>();
        }
        [HttpPost]
        [Route("api/Account/Register")]
        // регистрация пользователя
        public async Task<IActionResult> Register([FromBody]RegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                DAL.Entity.User user = new DAL.Entity.User
                {
                    Email = model.Email,
                    UserName = model.Email
                };
                // Добавление нового пользователя
                var result = await _userManager.CreateAsync(user,
                model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    // установка куки

                    await _signInManager.SignInAsync(user, false);

                    var msg = new
                    {
                        message = "Добавлен новый пользователь: " +
                    user.UserName
                    };
                    logger.LogInformation("Добавлен новый пользователь: " +
                    user.UserName);
                    return Ok(msg);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty,
                        error.Description);
                    }
                    var errorMsg = new
                    {
                        message = "Пользователь не добавлен.",
                        error = ModelState.Values.SelectMany(e =>
                        e.Errors.Select(er => er.ErrorMessage))
                    };
                    return StatusCode(203, errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Неверные входные данные.",
                    error = ModelState.Values.SelectMany(e =>
                    e.Errors.Select(er => er.ErrorMessage))
                };

                return StatusCode(203, errorMsg);
            }
        }
        [HttpPost]
        [Route("api/Account/Login")]
        // вход пользователя
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    string role = "user";
                    if (await _userManager.IsInRoleAsync(user, "admin"))
                    {
                        role = "admin";
                        logger.LogInformation("Вход администратора " + model.Email);  
                    }
                    var msg = new
                    {
                        messange = "Выполнен вход пользователем: " + model.Email,
                        role = role
                    };
                    return Ok(msg);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    var errorMsg = new
                    {
                        message = "Вход не выполнен.",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return StatusCode(203, errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e =>
                    e.Errors.Select(er => er.ErrorMessage))
                };
                return StatusCode(203, errorMsg);
            }
        }
        [HttpPost]
        [Route("api/Account/LogOff")]
        // выход из аккаунта
        public async Task<IActionResult> LogOff()
        {
            // Удаление куки
            await _signInManager.SignOutAsync();
            var msg = new
            {
                message = "Выполнен выход."
            };
            return Ok(msg);
        }

        [HttpPost]
        [Route("api/Account/checkRole/")]
        // проверка роли текущего пользователя
        public async Task<IActionResult> CheckRole()
        {
            DAL.Entity.User user = await GetCurrentUserAsync();
            string role;
            if (user == null)
            {
                role = "";
            }
            else
            {
                if (await _userManager.IsInRoleAsync(user, "admin"))
                {
                    role = "admin";
                }
                else
                {
                    role = "user";
                }
            }
            var msg = new
            {
                role = role
            };
            return Ok(msg);
        }
        private Task<DAL.Entity.User> GetCurrentUserAsync() =>
        _userManager.GetUserAsync(HttpContext.User);

        [HttpPost]
        [Route("api/Account/isAuthenticated")]
        // проверка наличия вошедшего пользователя
        public async Task<IActionResult> LogisAuthenticatedOff()
        {
            DAL.Entity.User usr = await GetCurrentUserAsync();
            var message = usr == null ? "Вы Гость. Пожалуйста, выполните вход." : "Вы вошли как: " + usr.UserName;
            var msg = new
            {
                message
            };
            return StatusCode(203, msg);
        }
        // получение текущего пользователя
    }
}
