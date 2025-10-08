using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface ITeamInvitationService
    {
        Task<TeamInvitation> SendInvitationAsync(TeamInvitationDTO invitationDto);
        Task AccepteInvitationAsync(int invId);
        Task<List<MemberTeamInvitationDTOO>> GetUserTeamInvitationsAsync(int userId);
        Task<UserTeamInvitationsResponseDto> GetUserTeamInvitationsNbrAsync(int userId, int adminId);
        Task RefuserInvitation(int id);


    }
}
