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
    public class HaltController : Controller
    {
        DBOperations db = new DBOperations();
        ILogger logger; // логгер

        public HaltController()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });

            logger = loggerFactory.CreateLogger<HaltController>();
        }
        [HttpGet]
        public List<HaltDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<HaltDTO>();
            }
            List<HaltDTO> result = db.GetHalts();
            if (result == null)
            {
                result = new List<HaltDTO>();
            }
            return result;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var halt = db.GetHalt(id);
            if (halt == null)
            {
                return NotFound();
            }
            return Ok(halt);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HaltDTO halt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            halt.halt_id = db.AddHalt(halt);
            if (halt.halt_id == -1)
            {
                return BadRequest();
            }
            logger.LogInformation("Добавление остановки с номером " + halt.halt_id);
            return CreatedAtAction("Get", new { id = halt.halt_id }, halt);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] HaltDTO halt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.UpdateHalt(halt);
            logger.LogInformation("Обновление остановки с номером " + halt.halt_id);
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
            var halt = db.GetHalt(id);
            if (halt == null)
            {
                return NotFound();
            }
            halt.hidden = (halt.hidden + 1) % 2; // смена статуса
            db.UpdateHalt(halt);
            logger.LogInformation("Смена видимости остановки с номером " + halt.halt_id);
            return NoContent();
        }
    }
}
