using Auth.Dtos;
using Auth.Interfaces;
using Auth.Mappers;
using Auth.Model;
using Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IAdminService _adminService;
        private readonly IEmployerService _employerService;
        private readonly ITokenRepository _tokenRepository;
        private readonly ValidateTokenDirector _validateTokenDirector;

        public LoginController(IUserService userService, IJwtService jwtService, IAdminService adminService, IEmployerService employerService, ITokenRepository tokenRepository, ValidateTokenDirector validateTokenDirector )
        {
            _userService = userService;
            _jwtService = jwtService;
            _adminService = adminService;
            _employerService = employerService;
            _tokenRepository = tokenRepository;
            _validateTokenDirector = validateTokenDirector;
        }

        // User Login - Issue Access Token and Refresh Token
        [HttpPost("User")]
        public async Task<IActionResult> UserLogin([FromBody] UserLogin model, [FromServices] Func<string, ILoginService> loginServiceFactory)
        {
            try
            {
                // Determine login type: Normal, Facebook, or Google
                string loginType = !string.IsNullOrEmpty(model.FacebookToken) ? "facebook" :
                           !string.IsNullOrEmpty(model.GoogleToken) ? "google" : "normal";

                // Get the correct login service
                var loginService = loginServiceFactory(loginType);

                // Validate user login
                GetUserDto? user = await loginService.ValidateUserAsync(model);
                if (user == null)
                {
                    return StatusCode(403, "Invalid login credentials.");
                }

                string refreshToken = await _jwtService.GenerateRefreshToken();
                if (refreshToken == null)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }

                ValidateToken Vtoken = _validateTokenDirector.BuildUserToken(user.Id, "User", model.AdminId, user.Nom, user.Prenom, user.Image, DateTime.Now, refreshToken);
                await _tokenRepository.AddTokenAsync(Vtoken);

                string token = _jwtService.GenerateAccessToken(user.Id, "User", model.AdminId, user.Nom, user.Prenom, user.Image);

                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    SameSite = SameSiteMode.None
                });

                return Ok(new { token, refreshToken });
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
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //admin
        [HttpPost("Admin")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginDto model)
        {
            try
            {
                GetEmpLogin adm = await _adminService.LoginAdminAsync(model);
                if (adm == null)
                {
                    return StatusCode(403, "Invalid login credentials.");
                }

                string refreshToken = await _jwtService.GenerateRefreshToken();
                if (refreshToken == null)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }
                ValidateToken Vtoken = _validateTokenDirector.BuildEmpToken(adm.Id, "Admin", model.AdminId, adm.Nom, adm.Prenom, DateTime.Now, refreshToken,null);
                await _tokenRepository.AddTokenAsync(Vtoken);

                // Generate both the access token and refresh token
                string token = _jwtService.GenerateAccessToken(adm.Id, "Admin", model.AdminId, adm.Nom, adm.Prenom, null);

                // Store refresh token in HttpOnly cookie for security
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    SameSite = SameSiteMode.None 
                });

                return Ok(new { token, refreshToken });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(403, "Invalid login credentials.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, "Invalid login credentials.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //Employer login
        [HttpPost("Employer")]
        public async Task<IActionResult> EmployerLogin([FromBody] EmployerLoginDto model)
        {
            try
            {
                GetEmpLogin Emp = await _employerService.LoginEmployerAsync(model);
                if (Emp == null)
                {
                    return StatusCode(403, "Invalid login credentials.");
                }

                string refreshToken = await _jwtService.GenerateRefreshToken();
                if (refreshToken == null)
                {
                    return StatusCode(500, "An error occurred while logging in.");
                }
                ValidateToken Vtoken = _validateTokenDirector.BuildEmpToken(Emp.Id, "Employee", model.AdminId, Emp.Nom, Emp.Prenom, DateTime.Now, refreshToken, Emp.Image);
                await _tokenRepository.AddTokenAsync(Vtoken);

                // Generate both the access token and refresh token
                string token = _jwtService.GenerateAccessToken(Emp.Id, "Employee", model.AdminId, Emp.Nom, Emp.Prenom, Emp.Image);

                // Store refresh token in HttpOnly cookie for security
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    SameSite = SameSiteMode.None  // Set expiration time for refresh token (e.g., 30 days)
                });

                return Ok(new { token, refreshToken });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(403, "Invalid login credentials.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, "Invalid login credentials.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        // Refresh Token - Issue new Access Token using the Refresh Token
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            // Retrieve the refresh token from the HttpOnly cookie
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided");
            }

            ValidateToken token = await _tokenRepository.GetValidateTokenAsync(refreshToken);
            if(token == null)
            {
                return Unauthorized("The provided not valid");
            }
            // Generate a new access token
            string newToken = _jwtService.GenerateAccessToken(token.UserId, token.Role, token.AdminId, token.Nom, token.Prenom, token.ImageUrl);

            return Ok(new { token = newToken });
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Remove the refresh token from the HttpOnly cookie
            Response.Cookies.Delete("refreshToken");

            // Optionally, you can also invalidate the access token here (e.g., by blacklisting it, if necessary)

            return Ok("Logged out successfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTokenAsync([FromBody] UpdateTokenDto dto )
        {
            ValidateToken token = TokenMapper.updateToModel(dto);
            try
            {
                var tk = await _jwtService.updateTokenAsync(token);
                if (tk != null)
                {
                    return NoContent();
                }
                return BadRequest();
            }catch(Exception ex)
            {
                return StatusCode(500, "eroor");
            }
            
        }

    }
}
