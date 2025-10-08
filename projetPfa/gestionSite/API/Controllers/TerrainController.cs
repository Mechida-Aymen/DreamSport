using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.API.Mappers;
using gestionSite.Core.Interfaces.TerrainInterfaces;
using gestionSite.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionSite.API.Filters;
using gestionSite.Infrastructure.Mappers;
using System.Data;

namespace gestionSite.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class TerrainController : ControllerBase
    {
        private readonly ITerrainService _terrainService;

        public TerrainController(ITerrainService terrainService)
        {
            _terrainService = terrainService;
        }

        // Consulter tous les terrains d'un administrateur
        [HttpGet("{AdminId}")]
        public async Task<ActionResult<IEnumerable<Terrain>>> GetTerrainsByAdminAsync(int AdminId)
        {
            if (AdminId <= 0)
            {
                return BadRequest("Invalid admin ID. It must be greater than 0.");
            }

            var terrains = await _terrainService.GetAllTerrainsByAdminAsync(AdminId);

            if (terrains == null || !terrains.Any())
            {
                return NotFound($"No terrains found for admin with ID {AdminId}.");
            }

            return Ok(terrains);
        }

        // Ajouter un terrain
        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<Terrain>> AddTerrainAsync([FromBody] AddTerrainDto addTerrain)
        {
            if (addTerrain == null)
            {
                return BadRequest(new { errorMessage = "Invalid terrain data." });
            }

            var terrain = TerrainMapper.AddTerrainDtoToTerrain(addTerrain);
            try
            {
                var result = await _terrainService.AddTerrainAsync(terrain);

                if (result == null)
                {
                    return StatusCode(500, new { errorMessage = "An error occurred while adding the terrain." });
                }

                return Created("/api/Terrain/" + result.IdAdmin, result);

            }catch(InvalidOperationException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }


        }

        // Modifier un terrain
        [HttpPut]
        [ValidateModelAttribute]
        public async Task<ActionResult<Terrain>> UpdateTerrainAsync([FromBody] UpdateTerrainDto updateTerrain)
        {
            if (updateTerrain == null || updateTerrain.Id <= 0)
            {
                return BadRequest("Invalid terrain data.");
            }

            var terrain = TerrainMapper.UpdateTerrainDtoToTerrain(updateTerrain);
            var result = await _terrainService.UpdateTerrainAsync(terrain);

            if (result == null)
            {
                return BadRequest("Failed to update terrain.");
            }

            return Ok(result);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateTerrainStatusAsync([FromBody] UpdateStatusDto dto)
        {
            if(dto == null) { return BadRequest("No data provided"); }

            try
            {
                Terrain terrain = await _terrainService.UpdateTerrainStatusAsync(dto);
                if (terrain == null)
                {
                    return StatusCode(500, new { errorMessage = "an error happen while updating the court" });
                }
                return NoContent();
            }catch(KeyNotFoundException ex)
            {
                return NotFound(new {errorMessage=ex.Message});
            }
        }

        // Supprimer un terrain
        [HttpDelete("{id}/{AdminId}")]
        public async Task<ActionResult<Terrain>> DeleteTerrainAsync(int id, int adminId)
        {
            if (id <= 0)
            {
                return BadRequest("The ID must be greater than 0.");
            }

            var result = await _terrainService.DeleteTerrainAsync(id,adminId);

            if (result == null)
            {
                return NotFound("Terrain not found.");
            }

            return Ok(result);
        }

        [HttpGet("by-id/{id}/{AdminId}")]
        public async Task<IActionResult> GetTerrainByIdWithStatusAsync(int id, int adminId)
        {
            var terrain = await _terrainService.GetTerrainByIdWithStatusAsync(id, adminId);
            if (terrain == null)
                return NotFound();

            return Ok(terrain);
        }
    }
}

