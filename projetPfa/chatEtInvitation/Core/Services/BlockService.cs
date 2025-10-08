using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;

namespace chatEtInvitation.Core.Services
{
    public class BlockService : IBlockService
    {
        private readonly IBloqueListRepository _blockRepository;
        private readonly IUserService _userService;

        public BlockService(IBloqueListRepository blockRepository, IUserService userService)
        {
            _blockRepository = blockRepository;
            _userService = userService;
        }

        public async Task BlockUserAsync(int currentUserId, int userIdToBlock, int admin)
        {
            // Vérifier si l'utilisateur existe
            var userExists = await _userService.FetchUserAsync(userIdToBlock, admin);
            var userExistsInv = await _userService.FetchUserAsync(currentUserId, admin);

            if (userExists == null || userExists.Id == 0 || userExistsInv==null|| userExistsInv.Id==0)
                throw new ArgumentException("Utilisateur introuvable");

            await _blockRepository.BlockUserAsync(userIdToBlock, currentUserId);
        }

        public async Task UnblockUserAsync(int currentUserId, int userIdToUnblock, int adminId)
        {
            var userExists = await _userService.FetchUserAsync(userIdToUnblock, adminId);
            var userExistsInv = await _userService.FetchUserAsync(currentUserId, adminId);

            if (userExists == null || userExists.Id == 0 || userExistsInv == null || userExistsInv.Id == 0)
                throw new ArgumentException("Utilisateur introuvable");
            await _blockRepository.UnblockUserAsync(userIdToUnblock, currentUserId);
        }

        public async Task<List<int>> GetBlockedUsersAsync(int currentUserId)
        {
            return await _blockRepository.GetBlockedUsersAsync(currentUserId);
        }

        public async Task<bool> IsUserBlockedAsync(int user1, int user2)
        {
            return await _blockRepository.IsUserBlockedAsync(user1, user2);
        }

    }
}
