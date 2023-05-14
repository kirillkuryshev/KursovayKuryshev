using BLL.DTO;
using BLL.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CruiseController : Controller
    {
        ILogger logger; // логгер
        CruiseOperations cruiseOperations = new CruiseOperations();
        DBOperations db = new DBOperations();

        public CruiseController()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<CruiseController>();
        }
        [HttpGet]
        public List<CruiseDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<CruiseDTO>();
            }
            return db.GetCruises();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cruise = db.GetCruise(id);
            if (cruise == null)
            {
                return NotFound();
            }
            return Ok(cruise);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CruiseDTO cruise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            cruise.CruiseId = db.AddCruise(cruise);
            if (cruise.CruiseId == -1)
            {
                return BadRequest();
            }
            logger.LogInformation("Добавлен рейс с номером " + cruise.CruiseId);
            return CreatedAtAction("Get", new { id = cruise.CruiseId }, cruise);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CruiseDTO cruise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!cruiseOperations.Update(cruise))
            {
                return BadRequest();
            }
            logger.LogInformation("Обновлен рейс с номером" + cruise.CruiseId);
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
            var cruise = db.GetCruise(id);
            if (cruise == null)
            {
                return NotFound();
            }
            cruise.Hidden = (cruise.Hidden + 1) % 2;
            if (!db.UpdateCruise(cruise))
            {
                return BadRequest();
            }
            logger.LogInformation("Смена видимости рейса с номером" + id);
            return NoContent();
        }
    }
}
