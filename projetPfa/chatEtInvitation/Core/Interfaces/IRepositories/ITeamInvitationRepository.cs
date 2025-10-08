using chatEtInvitation.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace chatEtInvitation.Core.Interfaces.IRepositories
{
    public interface ITeamInvitationRepository
    {
        Task<TeamInvitation> GetExistingInvitationAsync(int emetteur, int recepteur);
        Task AddInvitationAsync(TeamInvitation invitation);
        Task<TeamInvitation> GetInvitationByIdAsync(int invId);
        Task DeleteInvitationAsync(TeamInvitation invitation);
        Task<List<TeamInvitation>> GetUserTeamInvitationsAsync(int userId);
        Task<int> GetUserTeamInvitationsCountAsync(int userId, int adminId);




    }
}
