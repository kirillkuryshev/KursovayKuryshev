using BLL.DTO;
using BLL.Models;
using BLL.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly UserManager<DAL.Entity.User> _userManager;
        private readonly SignInManager<DAL.Entity.User> _signInManager;
        private TicketOperations ticketOperations = new TicketOperations();
        ILogger logger; // логгер

        public TicketController(UserManager<DAL.Entity.User> userManager,
        SignInManager<DAL.Entity.User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<TicketController>();
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "user")]
        // проверяет возможность возврата билета по номеру
        public async Task<IActionResult> ReturnInfo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DAL.Entity.User user = await GetCurrentUserAsync();
            if (user == null)
            {
                return StatusCode(401);
            }
            return Ok(ticketOperations.CheckReturn(id, user.Email));
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "user")]
        // возврат билета
        public async Task<IActionResult> Return([FromBody] ReturnModel cost, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ticketOperations.Return(id, cost.Cost);
            logger.LogInformation("Возвращен билет " + id);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "user")]
        // получение списка рейсов с дополнительной информацией для переданных критериев поиска
        public List<TravellDTO> Travell([FromBody] SearchInfoDTO info)
        {
            if (!ModelState.IsValid)
            {
                return new List<TravellDTO>();
            }
            return ticketOperations.getTravells(info);
        }

        [HttpPost("{place}")]
        [Authorize(Roles = "user")]
        // покупка билета
        public async Task<IActionResult> Buy([FromBody] TravellDTO travell, [FromRoute] int place)
        {
            TicketOperations t = new TicketOperations();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DAL.Entity.User user = await GetCurrentUserAsync();
            if (user != null)
            {
                int code = t.Buy(travell, user.Email, place);
                string message = "";
                switch (code)
                {
                    case -1: return BadRequest();
                    case 0: { message = "Ошибка. Время отправки наступило"; break; };
                    case 1: { logger.LogInformation("Пользователь " + user.Email 
                        + " купил билет на рейс " + travell.CruiseId + " на " 
                        + travell.Date.ToString()); 
                            message = "Билет отправлен на почту"; break; };
                    case 2: { message = "Ошибка. Место уже занято"; break; }
                }
                var msg = new
                {
                    message = message,
                    code = code
                };
                return Ok(msg);
            }
            return StatusCode(401);
        }
        private Task<DAL.Entity.User> GetCurrentUserAsync() =>
        _userManager.GetUserAsync(HttpContext.User);
    }
}
