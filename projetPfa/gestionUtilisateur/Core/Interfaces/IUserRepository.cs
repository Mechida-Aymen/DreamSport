using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.Core.Models;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> AddUserManualyAsync(User _user);
        Task<bool> DoesUserWithPhoneExist(string phone,int id);
        Task<bool> DoesUserWithEmailExist(string email,int id);
        Task<bool> DoesUserWithUsernameExist(string username, int id);
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string email, int id);
        Task<User?> DoesUserWithFacebookExist(string id, int admin);
        
        Task<List<User>> SearchUsersAsync(string searchTerm, int id, int AdminId);

        Task<User?> DoesUserWithGoogleExist(string id, int admin);
        Task<PaginatedResponse<paginationUser>> GetUsersAsync(int skip, int limit, int adminId, bool? isBlocked = null, string searchTerm = null);
        Task<int> GetTotalCountAsync(bool? isBlocked = null);
        Task<bool> UpdateUserStatusAsync(int userId, bool isBlocked);


    }
}
