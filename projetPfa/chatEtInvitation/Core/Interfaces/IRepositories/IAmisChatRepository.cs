using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IAmisChatRepository
    {
        Task<AmisChat> CreateChatAsync(AmisChat chat);

    }
}
