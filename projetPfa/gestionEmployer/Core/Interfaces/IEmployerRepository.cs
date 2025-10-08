using gestionEmployer.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace gestionEmployer.Core.Interfaces

{
    public interface IEmployeeRepository
    {
        bool Exists(Expression<Func<Employer, bool>> predicate);
        Task<Employer> GetEmployeeByIdAsync(int id);
        Task<Employer> EmployerByCINAsync(string cin, int adminId);
        Task<Employer> EmployerByUsernameAsync(string username, int adminId);
        Task<Employer> EmployerByPhoneAsync(string phone, int adminId);

        Task<IEnumerable<Employer>> GetEmployesByAdminIdAsync(int idAdmin);

        Task<Employer> AddEmployeeAsync(Employer employee);
        Task<Employer?> UpdateEmployeeAsync(Employer employee);

        Task<Employer> DeleteEmployeeAsync(int id);
        Task<Employer> EmployerByEmailAsync(string email, int adminId);
        Task<List<Employer>> SearchEmployeesAsync(string searchTerm);
    }
}
