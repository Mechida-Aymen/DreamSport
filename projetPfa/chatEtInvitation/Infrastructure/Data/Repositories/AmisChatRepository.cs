using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Models;
using gestionEmployer.Infrastructure.Data;

namespace chatEtInvitation.Infrastructure.Data.Repositories
{
    public class AmisChatRepository : IAmisChatRepository
    {
        private readonly AppDbContext _context;

        public AmisChatRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<AmisChat> CreateChatAsync(AmisChat chat)
        {
            await _context.AmisChats.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }
    }
}
