using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface ITeamMessageRepository
    {

        Task<List<TeamChatMessage>> GetTeamConversationAsync(int Id);
        Task<TeamChat> ExisteChatTeamAsync(int teamchatId);
        Task<TeamChatMessage> GetLastTeamMessageAsync(int teamChatId);
        Task<string> GetUserMessageStatutAsync(int messageId, int userId);
        Task<int> GetUnreadMessagesCountAsync(int teamChatId, int userId);
        Task<PaginatedResponse<TeamChatMessage>> GetTeamConversationWithStatutsAsync(int teamChatId, int page = 1, int pageSize = 20);
        Task<TeamChatMessage> CreateTeamMessageAsync(TeamChatMessage message);
        Task AddMessageStatutAsync(MessageStatut statut);
        Task UpdateMessageStatutAsync(int messageId, int userId, int newStatutId);
        Task UpdateMessagesStatusAsync(List<int> messageIds, int userId, int newStatusId);

    }
}
