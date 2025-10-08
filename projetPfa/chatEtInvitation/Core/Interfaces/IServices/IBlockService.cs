namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IBlockService
    {
        Task BlockUserAsync(int currentUserId, int userIdToBlock,int adminId);
        Task UnblockUserAsync(int currentUserId, int userIdToUnblock , int adminId);
        Task<List<int>> GetBlockedUsersAsync(int currentUserId);
        Task<bool> IsUserBlockedAsync(int currentUserId, int targetUserId);
    }
}
