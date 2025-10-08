using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface IBloqueListRepository
    {
        Task<BloqueList> IsBlockedAsync(int Bloked, int BlokedBy);
        Task BlockUserAsync(int bloked, int blokedBy);
        Task UnblockUserAsync(int bloked, int blokedBy);
        Task<List<int>> GetBlockedUsersAsync(int userId);
        Task<bool> IsUserBlockedAsync(int bloked, int blokedBy);

    }
}
