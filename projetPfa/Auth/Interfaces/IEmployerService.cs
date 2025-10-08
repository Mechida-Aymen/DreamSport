using Auth.Dtos;

namespace Auth.Interfaces
{
    public interface IEmployerService
    {
        Task<GetEmpLogin> LoginEmployerAsync(EmployerLoginDto userLogin);
    }
}
