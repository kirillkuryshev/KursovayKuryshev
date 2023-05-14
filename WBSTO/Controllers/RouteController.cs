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
    public class RouteController : Controller
    {
        DBOperations db = new DBOperations();
        public RouteController()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
        }
        [HttpGet]
        public List<RouteDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<RouteDTO>();
            }
            return db.GetRoutes();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var Route = db.GetRoute(id);
            if (Route == null)
            {
                return NotFound();
            }
            return Ok(Route);
        }
    }
}
