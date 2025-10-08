using gestionSite.API.DTOs.FAQDtos;
using gestionSite.Core.Models;
using gestionSite.API.Filters;
using gestionSite.API.Mappers;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace gestionSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerrainStatusController : ControllerBase
    {
        private readonly ITerrainStatusService _statusService;

        public TerrainStatusController(ITerrainStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpGet("{AdminId}")]
        public async Task<ActionResult<IEnumerable<TerrainStatus>>> GetTerrainStatusAsync()
        {
            // Validate the adminId parameter
            
            // Retrieve FAQs using the service
            var Statuses = await _statusService.GetAllTerrainStatusAsync();

            // Handle null or empty result
            if (Statuses == null || !Statuses.Any())
            {
                return NotFound("No status exist.");
            }
            return Ok(Statuses);
        }

        [HttpPost]
        
        public async Task<ActionResult<TerrainStatus>> AddTerrainStatusAsync([FromBody]string name)
        {
            var status = new TerrainStatus
            {
                Libelle = name,
            };
            var result = await _statusService.AddTerrainStatusAsync(status);
            if (result == null)
            {
                return BadRequest("An error occurred while adding the Status");
            }

            return Created("/api/TerrainStatus", result);
        }

        [HttpDelete("{StatusId}/{AdminId}")]
        public async Task<ActionResult<TerrainStatus>> DeleteTerrainStatusAsync(int StatusId)
        {
            if (StatusId < 0)
            {
                return BadRequest("the Id must be positive");
            }
            var result = await _statusService.DeleteTerrainStatusAsync(StatusId);
            if (result == null)
            {
                return NotFound("Status not found");
            }

            return Ok(result);
        }
    }
}
