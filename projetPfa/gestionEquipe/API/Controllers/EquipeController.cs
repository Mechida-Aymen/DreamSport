using System.Text;
using gestionEquipe.API.DTOs;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionEquipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipeController : ControllerBase
    {
        private readonly IEquipeService _equipeService;


        public EquipeController(IEquipeService equipeService)
        {
            _equipeService = equipeService;
        }

        [HttpPost]
        public async Task<ActionResult<AddedEquipeDTO>> AddEquipeAsync([FromBody] AddEquipeDTO addEquipe)
        {
            var equipe = EquipeMapper.AddToModel(addEquipe);
            AddedEquipeDTO result = await _equipeService.AddEquipeAsync(equipe);
            if (result.Errors.Count() > 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
            //return Created("/api/faq/" + result.IdAdmin, result);
        }

        [HttpPut]
        public async Task<ActionResult<UpdatedEquipeDTO>> UpdateEquipeAsync([FromBody] UpdateEquipeDTO UpdateEquipe)
        {
            var equipe = EquipeMapper.UpdateEquipDTOtoEquipe(UpdateEquipe);
            UpdatedEquipeDTO result = await _equipeService.UpdateEquipeAsync(equipe);
            if (result.Errors.Count() > 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



        //   supprimer une équipe avec ses membres
        [HttpDelete("{equipeId}/{AdminId}")]
        public async Task<IActionResult> SupprimerEquipe(int equipeId)
        {
            try
            {
                await _equipeService.SupprimerEquipeAvecMembresAsync(equipeId);
                return NoContent(); // Réponse 204 No Content lorsque la suppression est réussie
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // En cas d'erreur, retour d'une réponse 400 BadRequest
            }
        }

        [HttpPut("ChangerCapitaine")]
        public async Task<ActionResult<UpdatedEquipeDTO>> TransferCaptain([FromBody] ChangerCapitaineEquipeDTO transferDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equipe = EquipeMapper.ChangerCapitainDTOToEquipe(transferDto);

            var result = await _equipeService.TransferCaptaincyAsync(equipe);

            if (result.Errors.Count() > 0)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }

        [HttpPost("capitaine-quitte")]
        public async Task<IActionResult> CapitainQuitte([FromBody] CapitainQuitteDTO dto)
        {
            try
            {
                await _equipeService.CapitainQuitteAsync(dto.CapitaineId, dto.EquipeId);
                return Ok(new { message = "Le capitaine a quitté et un nouveau capitaine a été désigné." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403,new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{teamId}/{AdminId}")]
        public async Task<IActionResult> GetTeamAsync(int teamId)
        {
            try
            {
                IEnumerable<int> MembersIds = await _equipeService.GetTeamAsync(teamId);
                return Ok(MembersIds);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("get/{teamId}/{AdminId}")]
        public async Task<IActionResult> GetTeam(int teamId)
        {
            var result = await _equipeService.GetEquipe(teamId);

            return Ok(result);
        }

        //--------------
        [HttpGet("check-membership/{userId}/{adminId}")]
        public async Task<ActionResult<UserTeamMembershipResponseDto>> CheckUserTeamMembershipAsync(int userId, int adminId)
        {
            var result = await _equipeService.CheckUserTeamMembershipAsync(userId, adminId);
            return Ok(result);
        }
    }
}
