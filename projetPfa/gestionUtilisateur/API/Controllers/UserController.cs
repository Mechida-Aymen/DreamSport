using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using gestionUtilisateur.API.Filters;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using gestionUtilisateur.Infrastructure.Data;
namespace gestionUtilisateur.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordUserService _passwordUserService;
        private readonly IMailService _mailService;

        public UserController(IUserService userService, IPasswordUserService passwordUserService)
        {
            _userService = userService;
            _passwordUserService = passwordUserService;
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<ActionResult<ReturnAddedUserManualy>> AddUserManualyAsync([FromBody] AddUserManualyDTO _userDto)
        {
            var user = UserMapper.AddUserToUser(_userDto);
            var result = await _userService.AddUserManualyAsync(user);
            if (result.errors.Count == 0)
            {
                return Created("/api/users/" + result.AdminId, result);

            }
            return BadRequest(result);
        }


        [HttpPut("{id}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserDto dto)
        {
            if (id <= 0)
                return BadRequest("ID utilisateur invalide.");

            var result = await _userService.UpdateUserAsync(id, dto);
            if (result == null)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Profil mis à jour avec succès.", data = result });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUser dto)
        {
            User user = UserMapper.updateToUser(dto);

            try
            {
                ReturnUpdated emp = await _userService.updateUserAsync(user);
                if (emp.Errors.Count() > 0)
                {
                    return BadRequest(emp);
                }
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}/{AdminId}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
        [HttpPut("update-sport-data/{id}")]
        [ValidateModelAttribute]
        public async Task<IActionResult> UpdateSportDataAsync(int id, [FromBody] UpdateSportDataDTO dto)
        {
            if (id <= 0)
                return BadRequest("ID utilisateur invalide.");

            var result = await _userService.UpdateSportDataAsync(id, dto);
            if (!result)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Profil sportif mis à jour avec succès." });
        }

        [HttpPut("recover-password")]
        [ValidateModelAttribute]
        public async Task<IActionResult> RecupererPassword([FromBody] RecupererPasswordDTO dto)
        {

            // Appel au service
            var userDto = await _userService.RecupererPasswodAsync(dto);

            if (userDto.error != null)
            {
                return BadRequest(userDto);
            }
            return Ok(userDto);

        }

        [HttpGet("get/{id}/{AdminId}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            User user = await _userService.GetUserAsync(id);
            if (user == null) { return NotFound(); }
            return Ok(user);
        }

        [HttpGet("get-right/{id}/{AdminId}")]
        public async Task<IActionResult> GetUserConfAsync(int id, int AdminId)
        {
            try
            {
                ReturnedLoginDto dto = await _userService.GetUserConfAsync(id);
                if (dto == null)
                {
                    return StatusCode(500, "An error happen while handling your request");
                }
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { errorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error happen while handling your request");
            }
        }

        [HttpPut("ResetConteur/{id}")]
        public async Task<IActionResult> ResetConteurResAnnuler(int id)
        {
            var success = await _userService.ResetConteurResAnnulerAsync(id);
            if (!success)
                return NotFound($"Utilisateur avec l'ID {id} introuvable.");

            return Ok(new { message = "Conteur_res_annuler réinitialisé avec succès." });
        }

        [HttpPut("check-reservation-annule/{id}")]
        public async Task<IActionResult> CheckAndIncrementReservationAnnule(int id)
        {
            try
            {
                var success = await _userService.CheckAndIncrementReservationAnnuleAsync(id);
                if (!success)
                    return Ok(new { message = "Utilisateur déjà bloqué des réservations." });

                return Ok(new { message = "Le compteur des réservations annulées a été mis à jour." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "Une erreur interne s'est produite. Veuillez réessayer plus tard." });
            }

        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateUser([FromBody] LoginDto model)
        {
            try
            {
                ReturnedLoginDto dto = await _userService.Login(model);
                if (dto == null)
                {
                    return Forbid("cant login please contact the support");
                }
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error ");
            }
        }

        [HttpPost("Register-facebook")]
        public async Task<IActionResult> AddFacebookUserAsync([FromBody] FacebookUserDto dto)
        {
            User user = UserMapper.FacebookToUser(dto);
            var result = await _userService.AddUserManualyAsync(user);
            if (result.errors.Count == 0)
            {
                if (dto.type.Equals("facebook"))
                {
                    ReturnedLoginDto dtoo = await _userService.FacebookLoginAsync(dto.FacebookId, dto.AdminId);
                    return Ok(dtoo);
                }
                ReturnedLoginDto dtoa = await _userService.GoogleLoginAsync(dto.FacebookId, dto.AdminId);
                return Ok(dtoa);

            }
            return BadRequest(result);
        }

        [HttpPost("facebook-validate/{id}/{AdminId}/{type}")]
        public async Task<IActionResult> GetUserByFacebookAsync(string id, int AdminId, string type)
        {
            try
            {
                if (type.Equals("facebook"))
                {
                    ReturnedLoginDto dtoo = await _userService.FacebookLoginAsync(id, AdminId);
                    return Ok(dtoo);
                }

                ReturnedLoginDto dto = await _userService.GoogleLoginAsync(id, AdminId);
                return Ok(dto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error ");
            }
        }

        //--search user

        [HttpGet("search/{searchTerm}/{id}/{AdminId}")]
        public async Task<ActionResult<List<UserDto>>> SearchUsersAsync(string searchTerm, int id, int AdminId)
        {
            var result = await _userService.SearchUsersAsync(searchTerm, id, AdminId);
            return Ok(result);
        }

        //change password
        [HttpPut("changePassworduser")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordUserDto ChangePasswordUserDto)
        {
            try
            {
                // Vérification de l'ancien mot de passe
                var isValid = await _passwordUserService.VerifyOldUserPassword(ChangePasswordUserDto.AdminId, ChangePasswordUserDto.EmployerId, ChangePasswordUserDto.OldPassword);

                if (!isValid)
                    return BadRequest();

                // Changement du mot de passe
                await _passwordUserService.ChangeUserPassword(ChangePasswordUserDto.AdminId, ChangePasswordUserDto.EmployerId, ChangePasswordUserDto.NewPassword);

                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne: {ex.Message}");
            }
        }



        [HttpPost("pagination")]
        public async Task<ActionResult<PaginatedResponse<paginationUser>>> GetUsers([FromBody] paginationParams dto)
        {
            try
            {
                var result = await _userService.GetUsersPaginatedAsync(
                    dto.skip,
                    dto.limit,
                    dto.AdminId,
                    dto.isBlocked,
                    dto.searchTerm
                );
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{userId}/status")]
        public async Task<IActionResult> UpdateUserStatus(int userId, [FromBody] UpdateStatusDto dto)
        {
            try
            {
                var result = await _userService.UpdateUserStatusAsync(userId, dto.isBlocked);
                if (!result)
                {
                    return NotFound("User not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
