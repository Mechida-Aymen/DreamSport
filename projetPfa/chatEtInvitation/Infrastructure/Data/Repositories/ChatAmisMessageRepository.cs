using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class ChatAmisMessageRepository : IChatAmisMessageRepository
    {
        private readonly AppDbContext _context;

        public ChatAmisMessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AmisChat>> GetUserChatsAsync(int userId)
        {
            return await _context.AmisChats
                .Where(ac => ac.Member1 == userId || ac.Member2 == userId)
                .ToListAsync();
        }


        public async Task<AmisChat> GetChatAmisByIdAsync(int Id)
        {
            return await _context.AmisChats.FindAsync(Id);
        }

        public async Task<ChatAmisMessage> GetLastChatMessageAsync(int chatId)
        {
            return await _context.ChatAmisMessages
                .Where(m => m.ChatAmisId == chatId)
                .Include(m => m.Statuts)
                    .ThenInclude(ms => ms.Statut) 
                .OrderByDescending(m => m.when)
                .FirstOrDefaultAsync();
        }
       

        public async Task<ChatAmisMessage> CreateAmisMessageAsync(ChatAmisMessage message)
        {
            await _context.ChatAmisMessages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task AddMessageStatutAsync(MessageStatut statut)
        {
            await _context.MessageStatuts.AddAsync(statut);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadMessagesCountAsync(int chatId, int userId)
        {
            return await _context.ChatAmisMessages
                .Where(m => m.ChatAmisId == chatId)
                .CountAsync(m => m.Statuts.Any(s =>
                    s.UtilisateurId == userId &&
                    s.Statut.libelle != "Seen" &&
                    s.IsTeam == false));
        }
        public async Task<PaginatedResponse<ChatAmisMessage>> GetConversationAsync(int chatAmisId, int page, int pageSize)
        {
            var query = _context.ChatAmisMessages
                .Where(m => m.ChatAmisId == chatAmisId)
                .Include(m => m.Statuts)
                    .ThenInclude(ms => ms.Statut)
                .OrderByDescending(m => m.when); // Changé OrderBy en OrderByDescending

            var totalCount = await query.CountAsync();
            var messages = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<ChatAmisMessage>
            {
                Items = messages,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<AmisChat> AmisChatCheck(int idMember1, int idMember2, int AdminId)
        {
            return await _context.AmisChats
                .Where(c =>
                    ((c.Member1 == idMember1 && c.Member2 == idMember2) ||
                     (c.Member1 == idMember2 && c.Member2 == idMember1)) &&
                     c.AdminId == AdminId)
                .FirstOrDefaultAsync();
        }



    }
}
