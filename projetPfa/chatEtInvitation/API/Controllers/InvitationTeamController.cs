using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using chatEtInvitation.API.DTOs;
using Microsoft.AspNetCore.SignalR;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationTeamController : ControllerBase
    {
        private readonly ITeamInvitationService _teamInvitationService;
        private readonly IHubContext<InvitationHub> _hubContext;

        public InvitationTeamController (ITeamInvitationService invitationService, IHubContext<InvitationHub> hubContext)
        {
            _teamInvitationService = invitationService;
            _hubContext = hubContext;

        }

        [HttpPost]
        public async Task<IActionResult> SendInvitation([FromBody] TeamInvitationDTO invitationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var invitation = await _teamInvitationService.SendInvitationAsync(invitationDto);

          
                // Envoyer via SignalR
                await _hubContext.Clients.Group(invitationDto.Recepteur.ToString())
                    .SendAsync("ReceiveTeamInvitation", invitation);
            

            return Ok ();
            
        }

        [HttpPost("accepter/{invitationId}")]
        public async Task<IActionResult> AccepterInvitation(int invitationId)
        {
            try
            {
                await _teamInvitationService.AccepteInvitationAsync(invitationId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error happen");
            }

        }

        // Endpoint API pour refuser une invitation
        [HttpDelete("Refuser/{id}/{AdminId}")]
        public async Task<IActionResult> RefuserInvitation(int id)
        {
            try
            {
                 await _teamInvitationService.RefuserInvitation(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error happen");
            }

        }


        // GET api/memberinvitations/{userId}

        [HttpGet("team-invitations/{userId}/{adminId}")]
        public async Task<ActionResult<UserTeamInvitationsResponseDto>> GetUserTeamInvitationsNbrAsync(int userId, int adminId)
        {
            var result = await _teamInvitationService.GetUserTeamInvitationsNbrAsync(userId, adminId);
            return Ok(result);
        }



    }
}
