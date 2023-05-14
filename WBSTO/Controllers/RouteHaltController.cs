using BLL.DTO;
using BLL.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteHaltController : Controller
    {
        DBOperations db = new DBOperations();
        ILogger logger; // логгер
        RouteHaltOperations routeHaltOperations = new RouteHaltOperations();

        public RouteHaltController()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<RouteHaltController>();
        }
        [HttpGet]
        public List<RouteHaltDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<RouteHaltDTO>();
            }
            List<RouteHaltDTO> result = db.GetRouteHalts();
            if (result == null)
            {
                return new List<RouteHaltDTO>();
            }
            return result.OrderBy(x => x.NumberInRoute).ToList();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var RouteHalt = db.GetRouteHalt(id);
            if (RouteHalt == null)
            {
                return NotFound();
            }
            return Ok(RouteHalt);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RouteHaltDTO RouteHalt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            RouteHalt.RouteHaltId = db.AddRouteHalt(RouteHalt);
            if (RouteHalt.RouteHaltId == -1)
            {
                return BadRequest();
            }
            logger.LogInformation("Добавление остановки маршрута с номером " + RouteHalt.RouteHaltId);
            return CreatedAtAction("Get", new { id = RouteHalt.RouteHaltId }, RouteHalt);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] RouteHaltDTO RouteHalt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (RouteHalt.NumberInRoute != 0) // у начальной остановки нельзя изменить стоимость и время
            {
                routeHaltOperations.Update(RouteHalt);
                if (!db.UpdateRouteHalt(RouteHalt))
                {
                    return BadRequest();
                }
                logger.LogInformation("Обновление остановки маршрута с номером " +
                    RouteHalt.RouteHaltId);
                return Ok();
            }
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var RouteHalt = db.GetRouteHalt(id);
            if (RouteHalt == null)
            {
                return NotFound();
            }
            RouteHalt.Hidden = (RouteHalt.Hidden + 1) % 2;
            routeHaltOperations.Hide(RouteHalt);
            if (db.UpdateRouteHalt(RouteHalt))
            {
                return BadRequest();
            }
            logger.LogInformation("Смена видимости остановки маршрута с номером " + 
                RouteHalt.RouteHaltId);
            return NoContent();
        }
    }
}
