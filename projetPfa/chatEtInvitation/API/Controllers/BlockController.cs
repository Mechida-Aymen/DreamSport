using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {

        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser(UserBlockDTO dto)
        {
            try
            {
                await _blockService.BlockUserAsync(dto.currentUserId, dto.userIdToBlock, dto.AdminId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors du blocage");
            }
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockUser(UserUnblockDTO dto)
        {
            try
            {
                await _blockService.UnblockUserAsync(dto.currentUserId, dto.userIdToUnblock , dto.AdminId);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors du déblocage");
            }
        }

        [HttpGet("blocked/{currentUserId}/{AdmineId}")]
        public async Task<ActionResult<List<int>>> GetBlockedUsers(int currentUserId, int AdminId)
        {
            try
            {
                var blockedUsers = await _blockService.GetBlockedUsersAsync(currentUserId);
                return Ok(blockedUsers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors de la récupération");
            }
        }

        [HttpGet("is-blocked/{targetUserId}/{currentUserId}/{AdminId}")]
        public async Task<ActionResult<bool>> IsUserBlocked(int targetUserId,int currentUserId,int AdminId)
        {
            try
            {
                var isBlocked = await _blockService.IsUserBlockedAsync(currentUserId, targetUserId);
                return Ok(isBlocked);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erreur lors de la vérification");
            }
        }

        [HttpGet("statusbetween/{userId1}/{userId2}/{AdminId}")]
        public async Task<IActionResult> GetBlockStatusBetweenUsers(int userId1, int userId2)
        {
            try
            {
                var user1BlockedUser2 = await _blockService.IsUserBlockedAsync(userId2, userId1);

                var user2BlockedUser1 = await _blockService.IsUserBlockedAsync(userId1, userId2);

                return Ok(new
                {
                    user1BlockedUser2,
                    user2BlockedUser1
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error checking block status");
            }
        }

    }
}
