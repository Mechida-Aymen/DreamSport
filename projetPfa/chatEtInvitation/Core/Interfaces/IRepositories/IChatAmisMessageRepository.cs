using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IChatAmisMessageRepository
    {
        Task<List<AmisChat>> GetUserChatsAsync(int userId);
        Task<ChatAmisMessage> GetLastChatMessageAsync(int chatId);
        Task<int> GetUnreadMessagesCountAsync(int chatId, int userId);
        Task<ChatAmisMessage> CreateAmisMessageAsync(ChatAmisMessage message);
        Task AddMessageStatutAsync(MessageStatut statut);
        Task<AmisChat> GetChatAmisByIdAsync(int Id);
        Task<PaginatedResponse<ChatAmisMessage>> GetConversationAsync(int chatAmisId,int page,int pageSize);
        Task<AmisChat> AmisChatCheck(int idMember1, int idMember2, int AdminId);
    }
}
