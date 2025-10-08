using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatTeamController : ControllerBase
    {
        private readonly IchatTeamService _teamChatService;
        private readonly IHubContext<ChatHub> _hubContext;


        public ChatTeamController(IchatTeamService teamChatService, IHubContext<ChatHub> hubContext)
        {
            _teamChatService = teamChatService;
            _hubContext = hubContext;
        }

        [HttpGet("{teamId}/members/{memberId}/{AdminId}")]
        public async Task<ActionResult<TeamChatReturnedDTO>> GetTeamChatInfo(int teamId, int memberId)
        {
            try
            {
                var result = await _teamChatService.GetTeamChatByIdAsync(teamId, memberId);

                if (result == null)
                {
                    return NotFound($"Aucun chat trouvé pour l'équipe {teamId}");
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                // Loguer l'erreur (ex: _logger.LogError(ex, "Erreur lors de la récupération du chat"))
                return StatusCode(500, "Une erreur est survenue lors du traitement de votre demande");
            }
        }

        [HttpGet("{teamId}/conversation/{AdminId}")]
        public async Task<ActionResult<PaginatedResponse<TeamMessageDTO>>> GetFullConversation(
            int teamId,
            int adminId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var result = await _teamChatService.GetFullTeamConversationAsync(teamId, adminId, page, pageSize);

                if (result == null)
                {
                    return NotFound($"Aucun chat trouvé pour l'équipe {teamId}");
                }

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue lors du traitement de votre demande");
            }
        }

        [HttpPost("mark-as-seen")]
        public async Task MarkMessagesAsSeenAsync(MarkerMessageDTO dto)
        {
            await _teamChatService.MarkMessagesAsSeenAsync(dto.messageIds, dto.userId);
        }

        public class MarkMessagesAsSeenRequest
        {
            public List<int> MessageIds { get; set; }
            public int UserId { get; set; }
            // AdminId n'est pas nécessaire dans le corps de la requête si vous l'utilisez pour l'authentification
        }



        [HttpPost("send")]
        public async Task<ActionResult<TeamMessageDTO>> SendMessage(
    [FromBody] SendTeamMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _teamChatService.SendTeamMessageAsync(messageDto, messageDto.AdminId);

                var rst= SignalRChatMapper.TeamMessageSignalRDTO(result);
                foreach (var member in result.TeamMemberIds)
                {
                    await _hubContext.Clients.Group(member.UserId.ToString())
                                       .SendAsync("ReceiveTeamMessage", rst);
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue lors de l'envoi du message");
            }
        }

        [HttpPost("mark-all-as-seen")]
        public async Task<ActionResult> MarkAllMessagesAsSeen(MarkAllAsSeenDTO dto)
        {
            try
            {
                await _teamChatService.MarkAllMessagesAsSeenAsync(dto.teamChatId, dto.userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error marking messages as seen");
            }
        }


    }
}
