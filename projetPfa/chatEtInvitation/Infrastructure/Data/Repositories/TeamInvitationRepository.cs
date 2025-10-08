using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamInvitationRepository : ITeamInvitationRepository
    {
        private readonly AppDbContext _context;

        public TeamInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamInvitation> GetExistingInvitationAsync(int emetteur, int recepteur)
        {
            return await _context.TeamInvitations
                .FirstOrDefaultAsync(i => i.Emetteur == emetteur && i.Recerpteur == recepteur);
        }

        public async Task AddInvitationAsync(TeamInvitation invitation)
        {
            await _context.TeamInvitations.AddAsync(invitation);
            await _context.SaveChangesAsync();
        }

        public async Task<TeamInvitation> GetInvitationByIdAsync(int invId)
        {
            return await _context.TeamInvitations.FindAsync(invId);
        }

        public async Task DeleteInvitationAsync(TeamInvitation invitation)
        {
            _context.TeamInvitations.Remove(invitation);
            await _context.SaveChangesAsync(); // Sauvegarder les changements dans la base de données
        }





        //get invitation et lister 
        public async Task<List<TeamInvitation>> GetUserTeamInvitationsAsync(int userId)
        {
            return await _context.TeamInvitations
                .Where(inv => inv.Recerpteur == userId) // Filtrer uniquement les invitations destinées à l'utilisateur
                .ToListAsync();
        }



        //Methode pour lister les invitation de team d'un membre avec le nombre 

        public async Task<int> GetUserTeamInvitationsCountAsync(int userId, int adminId)
        {
            return await _context.TeamInvitations
                .Where(inv => inv.Recerpteur == userId && inv.AdminId == adminId)
                .CountAsync();
        }

    }
}
