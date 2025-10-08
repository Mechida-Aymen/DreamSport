using Auth.Dtos;
using Auth.Interfaces;

namespace Auth.Services
{
    public class GoogleLoginAdapter : ILoginService
    {
        private readonly IUserService _userService;
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleLoginAdapter(IUserService userService, IGoogleAuthService googleAuthService)
        {
            _userService = userService;
            _googleAuthService = googleAuthService;
        }

        public async Task<GetUserDto?> ValidateUserAsync(UserLogin model)
        {
            FacebookUserDto facebookUser = await _googleAuthService.ValidateGoogleTokenAsync(model.GoogleToken);
            if (facebookUser == null)
                throw new UnauthorizedAccessException("Invalid Google token.");
            //hna triguel

            GetUserDto dto = await _userService.GetUserByFacebookIdAsync(facebookUser.FacebookId, model.AdminId, "google");
            if (dto == null)
            {
                return await _userService.AddUserAsync(facebookUser, model.AdminId, "google");
            }
            return dto;
        }
    }
}
