using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Exceptions;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Models;
using chatEtInvitation.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationMemberController : ControllerBase
    {

        private readonly IMemberInvitationService _memberInvitationService;
        private readonly IInvitationService _invitationService;
        private readonly IHubContext<InvitationHub> _hubContext;


        public InvitationMemberController(IMemberInvitationService memberInvitationService,IInvitationService invitationService, IHubContext<InvitationHub> hubContext)
        {
            _memberInvitationService = memberInvitationService;
            _invitationService = invitationService;
            _hubContext = hubContext;

        }

        


        [HttpPost("send")]
        public async Task<IActionResult> SendMemberInvitation([FromBody] AddInvMemberDto dto)
        {
            if (dto.Recerpteur == dto.Emetteur)
            {
                return BadRequest("You cannot send an invitation to yourself");
            }

            try
            {
                // Sauvegarder l'invitation en base de données
                MemberInvitation invitation = MemberInvitationMapper.AddDtoToModel(dto);

                 await _memberInvitationService.SendMemberInvitationAsync(invitation);


                // Envoyer via SignalR
                await _hubContext.Clients.Group(dto.Recerpteur.ToString())
                    .SendAsync("ReceiveInvitation", invitation);

                return Ok(new { message = "Invitation sent successfully!", invitation = invitation });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred.", error = ex.Message });
            }
        }



        // Endpoint API pour refuser une invitation
        [HttpDelete("Refuser/{id}/{AdminId}")]
        public async Task<IActionResult> RefuserInvitation(int id)
        {
            try
            {
                var success = await _invitationService.RefuserInvitation(id);

                if (success)
                {
                    return NoContent();
                }

            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return BadRequest();
            
        }

        //Get invitation By Id
        [HttpGet("Get/{id}")]

        public async Task<IActionResult> GetInvitationByIdAsync(int id)
        {
            //Appel du service pour get Invitation 
            var invitation = await _invitationService.GetInvitationByIdAsync(id);
            if (invitation == null)
            {
                // si l'invitation ne se trouve pas return 404 not found avec le message 
                return NotFound(new {message= "Invitation non trouvée" });
            }
            return Ok(invitation);
        }

        //Accepter invitation 


        [HttpPost("accepter/{invitationId}")]
        public IActionResult AccepterInvitation(int invitationId)
        {
            try
            {
                if (_invitationService.AccepterInvitationAsync(invitationId).Result==false)
                {
                    return StatusCode(500, new { message = "An error occurred while processing your request" });
                }
                return Ok(new
                {
                    success = true,
                    message = "Invitation accepted successfully",
                    data = new { invitationId }
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInvitations(int userId)
        {
            var invitations = await _memberInvitationService.GetUserInvitationsAsync(userId);
            if (invitations == null || invitations.Count == 0)
            {
                return NotFound(new { message = "Aucune invitation trouvée." });
            }

            return Ok(invitations);
        }


        // GET api/memberinvitations/{userId}

        [HttpGet("user-invitations/{userId}/{adminId}")]
        public async Task<ActionResult<UserInvitationsResponseDto>> GetUserInvitationsNbrAsync(int userId, int adminId)
        {

            var result = await _memberInvitationService.GetUserInvitationsNbrAsync(userId, adminId);
            return Ok(result);
        }


    }
}
