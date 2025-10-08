using gestionSite.API.DTOs.AnnoncesDtos;
using gestionSite.API.Filters;
using gestionSite.API.Mappers;
using gestionSite.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnoncesController : ControllerBase
    {
        private readonly IAnnoncesService _annoncesService;

        public AnnoncesController(IAnnoncesService annoncesService)
        {
            _annoncesService = annoncesService;
        }

        [HttpGet("{AdminId}")]
        public async Task<ActionResult<IEnumerable<Annonces>>> GetAnnoncesAsync(int AdminId)
        {
            // Validate the adminId parameter
            if (AdminId <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }
            // Retrieve Annonces using the service
            var annonces = await _annoncesService.GetAnnoncesByAdminAsync(AdminId);

            // Handle null or empty result
            if (annonces == null || !annonces.Any())
            {
                return NotFound($"No Annonce found for admin with ID {AdminId}.");
            }
            return Ok(annonces);
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<Annonces>> AddFAQAsync([FromBody] AjouterAnnoncesDto AddAnnonces)
        {
            var annonces = AnnoncesMappers.AjouterAnnoncesDtoToAnnonces(AddAnnonces);
            var result = await _annoncesService.AddAnnoncesAsync(annonces);
            if (result == null)
            {
                return BadRequest("An error occurred while adding the Annonce");
            }

            return Created("/api/annonces/" + result.IdAdmin, result);
        }

        [HttpDelete("{annoncesId}/{AdminId}")]
        public async Task<ActionResult<Annonces>> DeleteAnnoncesAsync(int annoncesId)
        {
            if (annoncesId < 0)
            {
                return BadRequest("the Id must be positive");
            }
            var result = await _annoncesService.DeleteAnnoncesAsync(annoncesId);
            if (result == null)
            {
                return NotFound("Annonces not found");
            }

            return Ok(result);
        }
    }
}
