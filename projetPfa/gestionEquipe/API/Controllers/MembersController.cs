using gestionEquipe.API.DTOs;
using gestionEquipe.API.Exceptions;
using gestionEquipe.API.Mappers;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using gestionEquipe.Core.Services;
using gestionEquipe.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gestionEquipe.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersService _membersService;

        public MembersController(IMembersService membersService)
        {
            _membersService = membersService;
        }

        [HttpDelete("{AdminId}")]
        public async Task<ActionResult> KickMemberAsync([FromBody] DeleteMemberDTO member)
        {
            var _member = MembersMapper.DeleteDTOtoModel(member);
            try
            {
                var deletedMember = await _membersService.KickMemberAsync(_member);
                return Ok(new { message = "Member removed successfully.", _member = deletedMember });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Handle when team or member is not found
            }
            
            catch (Exception ex)
            {
                // Handle any other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberToTeam([FromBody]AddMemberDTO memberDTo)
        {

            try
            {
                Members member = MembersMapper.AddMemberDTOtoModel(memberDTo);
                // Call the service method to add the member
                Members addedMember = await _membersService.AjouterMemberAsync(member);

                // If the member is successfully added, return Created response with the member's data
                return Ok(memberDTo);
            }
            catch (KeyNotFoundException ex)
            {
                // If the team is not found
                return NotFound(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                // If the member is already part of the team
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // If the member is already in another team or the team is full
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
            }
        }



    }
}
