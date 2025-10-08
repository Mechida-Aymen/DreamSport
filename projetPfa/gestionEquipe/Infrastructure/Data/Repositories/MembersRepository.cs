using gestionEquipe.Core.Interfaces;
using gestionEquipe.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionEquipe.Infrastructure.Data.Repositories
{
    public class MembersRepository : IMembersRepository

    {
        private readonly AppDbContext _context;

        public MembersRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<Members> KickkMemberAsync(Members member)
        {
            // Ensure that the entity is not already tracked by the context
            var trackedMember = _context.Memberss
                .FirstOrDefault(m => m.UserId == member.UserId && m.EquipeId == member.EquipeId);

            if (trackedMember != null)
            {
                // If the member is tracked, detach it to avoid tracking conflicts
                _context.Entry(trackedMember).State = EntityState.Detached;
            }

            // Now safely remove the member entity
            _context.Memberss.Remove(member);

            // Save changes to ensure the deletion is committed
            await _context.SaveChangesAsync();

            // Return the removed member object as confirmation
            return member;
        }

        public async Task<Members> KickMemberAsync(Members member)
        {
            _context.Memberss.Remove(member);

            // Save changes to ensure the deletion is committed
            await _context.SaveChangesAsync();

            // Return the removed member object as confirmation
            return member;
        }

        public async Task<bool> ExistInTeamAsync(Members member)
        {
            return await _context.Memberss.AnyAsync(m => m.UserId == member.UserId && m.EquipeId == member.EquipeId);
            
        }
        public async Task<bool> ExistInTeamWithIdAsync(int IdMembre,int idEquipe)
        {
            return await _context.Memberss.AnyAsync(m => m.UserId == IdMembre && m.EquipeId == idEquipe);

        }

        public async Task<Members> AddMemberAsync(Members member)
        {
            var result = await _context.Memberss.AddAsync(member); // Add entity
            
            return result.Entity; // Return the saved entity
        }

        public async Task<Members> AddMemberSaveAsync(Members member)
        {
            var result = await _context.Memberss.AddAsync(member); // Add entity
            _context.SaveChanges();
            return result.Entity; // Return the saved entity
        }

        public async Task<int> CountTeamMembersAsync(int EquipeId)
        {
            return await _context.Memberss.CountAsync(e => e.EquipeId == EquipeId);
        }

        public async Task<List<Members>> GetTeamMembersAsync(int equipeId)
        {
            List<Members> ls =  await _context.Memberss
                .Where(m => m.EquipeId == equipeId)
                .ToListAsync();
            return ls;
        }


        public async Task<int> CountTeamsForMemberAsync(int MemberId)
        {
            return await _context.Memberss
        .CountAsync(t => t.UserId == MemberId);
        }
    }
}
