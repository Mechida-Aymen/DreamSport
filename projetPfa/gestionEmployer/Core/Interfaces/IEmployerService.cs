using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employer> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<GetEmployeeDTO>> GetEmployesByAdminIdAsync(int idAdmin);

        Task<ReturnAddedEmployee> AddEmployeeAsync(Employer employee);
        Task<ReturnUpdatedEmpDto?> UpdateEmployeeAsync(Employer employee);
        Task<Employer> DeleteEmployeeAsync(int id);
        Task<Employer> ModifyProfileAsync(Employer employer);
        Task<SendLoginEmployeeDto> ValidateLogin(EmployerLoginDto login);
        Task<IEnumerable<GetEmployeeDTO>> SearchEmployeesAsync(string searchTerm);
        Task<ReturnForgotPasswordDTO> RecupererPasswodAsync(recoverPass dto);


    }
}
