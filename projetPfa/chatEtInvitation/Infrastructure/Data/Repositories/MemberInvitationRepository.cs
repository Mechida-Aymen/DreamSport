using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class MemberInvitationRepository : IMemberInvitationRepository
    {
        private readonly AppDbContext _context;

        public MemberInvitationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MemberInvitation> GetMemberInvitationAsync(int emetteur, int recepteur)
        {
            return await _context.MemberInvitations
                .FirstOrDefaultAsync(i => i.Emetteur == emetteur && i.Recerpteur == recepteur);
        }

        public async Task AddInvitationAsync(MemberInvitation invitation)
        {
            _context.MemberInvitations.Add(invitation);
            await _context.SaveChangesAsync();
        }

        // Méthode pour refuser (supprimer) une invitation
        public async Task<bool> RefuserInvitation(MemberInvitation inv)
        {

            _context.MemberInvitations.Remove(inv);
            await _context.SaveChangesAsync(); // Sauvegarder les changements dans la base de données

            return true; // Invitation refusée et supprimée avec succès
        }

        /// Get invitation by id 

        public async Task<MemberInvitation> GetInvitationByIdAsync(int invitationId)
        {
            // Récupérer un MemberInvitation en utilisant l'ID
            return await _context.MemberInvitations
                                 .FirstOrDefaultAsync(i => i.Id == invitationId);
        }

        //Accepter invitation 

        // Méthode pour accepter une invitation
        public async Task<MemberInvitation> AccepterInvitationAsync(int invitationId)
        {
            // Récupérer l'invitation par ID
            var invitation = await GetInvitationByIdAsync(invitationId);

            _context.MemberInvitations.Remove(invitation);
            await _context.SaveChangesAsync();

            return invitation; // Invitation acceptée et chat créé avec succès
        }


        public async Task<List<MemberInvitation>> GetUserInvitationsAsync(int userId)
        {
            return await _context.MemberInvitations
                .Where(inv => inv.Recerpteur == userId) // Filtrer uniquement les invitations destinées à l'utilisateur
                .ToListAsync();
        }


        //Methode pour lister les invitation d'un membre avec le nombre 

        public async Task<int> GetUserInvitationsCountAsync(int userId, int adminId)
        {
            return await _context.MemberInvitations
                .Where(inv => inv.Recerpteur == userId && inv.AdminId == adminId)
                .CountAsync();
        }

    }
}

