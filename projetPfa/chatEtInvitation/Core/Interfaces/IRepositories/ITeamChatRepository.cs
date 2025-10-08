using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface ITeamChatRepository
    {
        Task<TeamChat> CreateChatAsync(TeamChat chat);
    }
}
