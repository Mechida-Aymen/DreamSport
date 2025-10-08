using gestionUtilisateur.API.DTOs;
using gestionUtilisateur.API.Mappers;
using gestionUtilisateur.Core.Interfaces;
using gestionUtilisateur.Core.Models;
using gestionUtilisateur.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace gestionUtilisateur.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context; // Replace with your DbContext name

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AddUserManualyAsync(User _user)
        {
            try
            {
                var result = await _context.Users.AddAsync(_user);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<bool> DoesUserWithEmailExist(string email, int id)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.IdAdmin == id);
        }

        public async Task<bool> DoesUserWithPhoneExist(string phone, int id)
        {
            return await _context.Users.AnyAsync(u => u.PhoneNumber == phone && u.IdAdmin == id);
        }

        public async Task<bool> DoesUserWithUsernameExist(string username, int id)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && u.IdAdmin==id);
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email, int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IdAdmin == id);
        }

        public async Task<User?> DoesUserWithFacebookExist(string id, int admin)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.FacebookId == id && u.IdAdmin == admin);
        }


        //---- search user 

        public async Task<List<User>> SearchUsersAsync(string searchTerm, int id, int adminId)
        {
            var query = _context.Users
                .Where(u => u.IdAdmin == adminId && u.Id != id);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var terms = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                query = query.Where(u => terms.All(term =>
                    u.Username.Contains(term) ||
                    u.Nom.Contains(term) ||
                    u.Prenom.Contains(term) ||
                    u.Bio.Contains(term)
                ));
            }

            return await query.ToListAsync();
        }



        public async Task<User?> DoesUserWithGoogleExist(string id, int admin)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == id && u.IdAdmin == admin);
        }


        public async Task<PaginatedResponse<paginationUser>> GetUsersAsync(int skip, int limit, int adminId, bool? isBlocked = null, string searchTerm = null)
        {
            var query = _context.Users
                .Where(u => u.IdAdmin == adminId) // Always filter by adminId
                .AsQueryable();

            if (isBlocked.HasValue)
            {
                query = query.Where(u => u.IsReservationBlocked == isBlocked.Value);
            }

            // Add search functionality
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(u =>
                    u.Prenom.ToLower().Contains(searchTerm) ||
                    u.Nom.ToLower().Contains(searchTerm) ||
                    u.Username.ToLower().Contains(searchTerm)
                );
            }

            var totalCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            List<paginationUser> lol = users.Select(UserMapper.modelTopagination).ToList();
            return new PaginatedResponse<paginationUser>(lol, totalCount);
        }

        public async Task<int> GetTotalCountAsync(bool? isBlocked = null)
        {
            var query = _context.Users.AsQueryable();

            if (isBlocked.HasValue)
            {
                query = query.Where(u => u.IsReservationBlocked == isBlocked.Value);
            }

            return await query.CountAsync();
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isBlocked)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.IsReservationBlocked = isBlocked;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
