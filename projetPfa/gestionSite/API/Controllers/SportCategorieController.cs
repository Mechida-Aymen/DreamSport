using gestionSite.Core.Interfaces.SportInterfaces;
using gestionSite.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportCategorieController : ControllerBase
    {
        private readonly ISportService _service;

        public SportCategorieController(ISportService servicee)
        {
            _service = servicee;
        }

        [HttpGet("execute/{AdminId}")]
        public async Task<ActionResult<IEnumerable<Sport_Categorie>>> GetSportsAsync()
        {
            var sportss = await _service.GetSportsAsync();
            if (sportss == null)
            {
                return BadRequest();
            }
            return Ok(sportss);
        }
    }
}
