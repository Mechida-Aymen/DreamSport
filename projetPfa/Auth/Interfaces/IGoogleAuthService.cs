using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<FacebookUserDto> ValidateGoogleTokenAsync(string token);
    }
}
