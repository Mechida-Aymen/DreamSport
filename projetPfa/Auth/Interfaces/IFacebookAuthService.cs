using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IFacebookAuthService
    {
        Task<FacebookUserDto> ValidateFacebookTokenAsync(string facebookToken);
    }
}
