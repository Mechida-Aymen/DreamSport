using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.Core.Interfaces.IServices
{
    public interface IMemberInvitationService
    {
        Task SendMemberInvitationAsync(MemberInvitation invitation);
        Task<List<MemberInvitationDTO>> GetUserInvitationsAsync(int userId);
        Task<UserInvitationsResponseDto> GetUserInvitationsNbrAsync(int userId, int adminId);


    }
}
