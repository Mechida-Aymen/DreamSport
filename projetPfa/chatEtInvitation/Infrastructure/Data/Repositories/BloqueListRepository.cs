using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class BloqueListRepository : IBloqueListRepository
    {
        private readonly AppDbContext _context;

        public BloqueListRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BloqueList> IsBlockedAsync(int Bloked, int BlokedBy)
        {
            return await _context.BloqueList
                .FirstOrDefaultAsync(b => b.Bloked == Bloked && b.BlokedBy == BlokedBy);
        }

        public async Task BlockUserAsync(int bloked, int blokedBy)
        {
            if (!await _context.BloqueList.AnyAsync(b => b.Bloked == bloked && b.BlokedBy == blokedBy))
            {
                _context.BloqueList.Add(new BloqueList { Bloked = bloked, BlokedBy = blokedBy });
                await _context.SaveChangesAsync();
            }
        }

        public async Task UnblockUserAsync(int bloked, int blokedBy)
        {
            var block = await _context.BloqueList
                .FirstOrDefaultAsync(b => b.Bloked == bloked && b.BlokedBy == blokedBy);

            if (block != null)
            {
                _context.BloqueList.Remove(block);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetBlockedUsersAsync(int userId)
        {
            return await _context.BloqueList
                .Where(b => b.BlokedBy == userId)
                .Select(b => b.Bloked)
                .ToListAsync();
        }

        public async Task<bool> IsUserBlockedAsync(int bloked, int blokedBy)
        {
            return await _context.BloqueList
                .AnyAsync(b => b.Bloked == bloked && b.BlokedBy == blokedBy);
        }

    }
}
