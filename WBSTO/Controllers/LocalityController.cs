using BLL.DTO;
using BLL.Operations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WBSTO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalityController : Controller
    {
        DBOperations db = new DBOperations();
        [HttpGet]
        public List<LocalityDTO> GetAll()
        {
            if (!ModelState.IsValid)
            {
                return new List<LocalityDTO>();
            }
            List<LocalityDTO> result = db.GetLocalities();
            if (result == null)
            {
                result = new List<LocalityDTO>();
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
            var locality = db.GetLocality(id);
            if (locality == null)
            {
                return NotFound();
            }
            return Ok(locality);
        }
    }
}
