using chatEtInvitation.API.DTOs;
using chatEtInvitation.API.Mappers;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatAmisController : ControllerBase
    {
        private readonly IchatAmisService _chatAmisService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatAmisController(IchatAmisService chatAmisService, IHubContext<ChatHub> hubContext)
        {
            _chatAmisService = chatAmisService;
            _hubContext = hubContext;
        }

        [HttpGet("{userId}/{AdminId}")]
        public async Task<ActionResult<List<AmisChatReturnedDTO>>> GetAmisChatInfo(int userId)
        {
            try
            {
                var result = await _chatAmisService.GetAmisChatInfoAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Une erreur est survenue");
            }
        }

        [HttpPost("send")]
        public async Task<ActionResult<AmisMessageDTO>> SendMessage([FromBody] SendAmisMessageDTO messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _chatAmisService.SendAmisMessageAsync(messageDto);


                UserInfoSignalRDTO emet = SignalRChatMapper.UserToUserSignalRDTO(result.Emetteur);
                AmisMessageSignalRDTO rsltSignalr = SignalRChatMapper.AmisMessageSignalRDTO(result);

                rsltSignalr.emetteur = emet;
                

                await _hubContext.Clients.Group(result.RecepteurId.ToString())
                          .SendAsync("ReceiveAmisMessage", rsltSignalr);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Erreur lors de l'envoi du message");
            }
        }

        [HttpGet("{chatAmisId}/conversation/{AdminId}")]
        public async Task<ActionResult<PaginatedResponse<AmisMessageDTO>>> GetConversation(
          int chatAmisId,
          int adminId,
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 20)
        {
            try
            {
                var result = await _chatAmisService.GetAmisConversationAsync(chatAmisId, adminId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Loguer l'erreur
                return StatusCode(500, "Erreur lors de la récupération de la conversation");
            }
        }

        [HttpGet("isAmisChat/{idMember1}/{idMember2}/{AdminId}")]
        public async Task<IActionResult> SearchUsersAsync(int idMember1, int idMember2, int AdminId)
        {
            var result = await _chatAmisService.AmisChatCheck(idMember1, idMember2, AdminId);
            return Ok(result);
        }

    }
}
