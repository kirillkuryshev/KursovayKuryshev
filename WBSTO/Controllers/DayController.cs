using BLL.DTO;
using BLL.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayController : Controller
    {
        DBOperations db = new DBOperations();
        [HttpGet]
        public List<DayDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<DayDTO>();
            }
            List<DayDTO> result = db.GetDays();
            if (result == null)
            {
                result = new List<DayDTO>();
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
            var day = db.GetDay(id);
            if (day == null)
            {
                return NotFound();
            }
            return Ok(day);
        }
    }
}
