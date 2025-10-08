using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamMessageRepository : ITeamMessageRepository
    {
        private readonly AppDbContext _context;

        public TeamMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamChat> ExisteChatTeamAsync(int teamchatId)
        {
            return await _context.TeamChats.FirstOrDefaultAsync(t => t.TeamId == teamchatId);
        }

        // méthode pour récupérer le dernier message
        public async Task<TeamChatMessage> GetLastTeamMessageAsync(int teamChatId)
        {
            return await _context.TeamChatMessages
                .Where(m => m.TeamChatId == teamChatId)
                .OrderByDescending(m => m.when)
                .FirstOrDefaultAsync();
        }

        // méthode pour récupérer le statut d'un utilisateur pour un message
        public async Task<string> GetUserMessageStatutAsync(int messageId, int userId)
        {
            return await _context.MessageStatuts
                .Where(ms => ms.MessageId == messageId && ms.UtilisateurId == userId)
                .Include(ms => ms.Statut)
                .Select(ms => ms.Statut.libelle)
                .FirstOrDefaultAsync();
        }

        // méthode pour compter les messages non lus
        public async Task<int> GetUnreadMessagesUserCountAsync(int teamChatId, int userId)
        {
            return await _context.TeamChatMessages
                .Where(m => m.TeamChatId == teamChatId)
                .Where(m => m.Statuts.Any(s => s.UtilisateurId == userId && s.Statut.libelle != "Seen"))
                .CountAsync();
        }

        public async Task<int> GetUnreadMessagesCountAsync(int teamChatId, int userId)
        {
            return await _context.TeamChatMessages
                .Where(m => m.TeamChatId == teamChatId)
                // Correction: Utiliser 'Emetteur' au lieu de 'EmetteurId'
                .Where(m => m.Emetteur != userId)
                .Where(m => m.Statuts.Any(s => s.UtilisateurId == userId && s.Statut.libelle != "Seen"))
                .CountAsync();
        }

        public async Task<List<TeamChatMessage>> GetTeamConversationAsync(int Id)
        {
            return await _context.TeamChatMessages
                .Where(cnv => cnv.TeamChatId == Id)
                .ToListAsync();
        }

        public async Task<PaginatedResponse<TeamChatMessage>> GetTeamConversationWithStatutsAsync(int teamChatId, int page = 1, int pageSize = 20)
        {
            var query = _context.TeamChatMessages
                .Where(m => m.TeamChatId == teamChatId)
                .Include(m => m.Statuts)
                    .ThenInclude(ms => ms.Statut)
                .OrderByDescending(m => m.when);

            var totalCount = await query.CountAsync();
            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<TeamChatMessage>
            {
                Items = messages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }


        public async Task<TeamChatMessage> CreateTeamMessageAsync(TeamChatMessage message)
        {
            await _context.TeamChatMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task AddMessageStatutAsync(MessageStatut statut)
        {
            await _context.MessageStatuts.AddAsync(statut);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateMessageStatutAsync(int messageId, int userId, int newStatutId)
        {
            var statut = await _context.MessageStatuts
                .FirstOrDefaultAsync(ms => ms.MessageId == messageId && ms.UtilisateurId == userId);

            if (statut != null)
            {
                statut.StatutId = newStatutId;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMessagesStatusAsync(List<int> messageIds, int userId, int newStatusId)
        {
            await _context.MessageStatuts
         .Where(ms => messageIds.Contains(ms.MessageId) && ms.UtilisateurId == userId)
         .ExecuteUpdateAsync(setters => setters.SetProperty(ms => ms.StatutId, newStatusId));
        }
        }
}