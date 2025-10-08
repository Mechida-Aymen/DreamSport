using gestionEmployer.API.DTOs.EmployeeDTO;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAuthService
    {
        Task UpdateTokenAsync(SendLoginEmployeeDto dto, int AdminId);
    }
}
