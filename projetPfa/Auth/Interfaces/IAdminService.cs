using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IAdminService
    {
        Task<GetEmpLogin> LoginAdminAsync(AdminLoginDto userLogin);
    }
}
