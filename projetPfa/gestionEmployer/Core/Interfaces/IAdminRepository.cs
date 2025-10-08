using gestionEmployer.Core.Models;

namespace gestionEmployer.Core.Interfaces
{
    public interface IAdminRepository
    {
        bool IsTenantValid(int tenantId);
        Admin? GetAdminByTenantId(int tenantId);
        Admin? AddAdmin(Admin admin);
        bool AdminExists(string nom, string login, string phoneNumber);
        Task<Admin> GetAdminAsync(int tenantId);
        Task<Admin> GetByLoginAsync(string login, int adminId);
        Task<Admin?> UpdateAdminAsync(Admin admin);
        Task<Admin> GetAdminByPhoneAsync(string phone, int adminId);
        Task<Admin> GetAdminByEmailAsync(string email, int adminId);

    }
}