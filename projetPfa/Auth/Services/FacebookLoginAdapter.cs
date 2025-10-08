using Auth.Dtos;
using Auth.Interfaces;

namespace Auth.Services
{
    public class FacebookLoginAdapter : ILoginService
    {
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IUserService _userService;

        public FacebookLoginAdapter(IFacebookAuthService facebookAuthService, IUserService userService)
        {
            _facebookAuthService = facebookAuthService;
            _userService = userService;
        }

        public async Task<GetUserDto?> ValidateUserAsync(UserLogin model)
        {
            FacebookUserDto facebookUser = await _facebookAuthService.ValidateFacebookTokenAsync(model.FacebookToken);
            if (facebookUser == null)
                throw new UnauthorizedAccessException("Invalid Facebook token.");
            //hna triguel
           
            GetUserDto dto = await _userService.GetUserByFacebookIdAsync(facebookUser.FacebookId, model.AdminId, "facebook");
            if(dto == null)
            {
                return await _userService.AddUserAsync(facebookUser, model.AdminId, "facebook");
            }
            return dto;
        }
    }
}
