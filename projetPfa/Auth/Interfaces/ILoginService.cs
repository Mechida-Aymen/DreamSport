using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface ILoginService
    {
        Task<GetUserDto?> ValidateUserAsync(UserLogin model);

    }
}
