using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class TeamChatRepository : ITeamChatRepository
    {
        private readonly AppDbContext _context;

        public TeamChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeamChat> CreateChatAsync(TeamChat chat)
        {
            await _context.TeamChats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }
    }
}
