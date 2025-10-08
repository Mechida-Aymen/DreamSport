using chatEtInvitation.API.DTOs;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IchatTeamService
    {
        Task<TeamChatReturnedDTO> GetTeamChatByIdAsync(int idEquipe, int idMember);
        Task<PaginatedResponse<TeamMessageDTO>> GetFullTeamConversationAsync(int teamId, int adminId, int page = 1, int pageSize = 20);
        Task<TeamMessageDTO> SendTeamMessageAsync(SendTeamMessageDTO messageDto, int adminId);
        Task MarkMessagesAsSeenAsync(List<int> messageIds, int userId);

        Task CreateTeamChat(int teamId, int adminId);
        Task MarkAllMessagesAsSeenAsync(int teamChatId, int userId);

    }
}
