using chatEtInvitation.Infrastructure.ExternServices.ExternDTOs;

namespace chatEtInvitation.Core.Interfaces.IExternServices
{
    public interface ITeamService
    {
        Task<List<int>> FetchMembersAsync(int TeamId, int adminId);
        Task<teamDTO> FetchTeamAsync(int teamId, int adminId);
    }
}
