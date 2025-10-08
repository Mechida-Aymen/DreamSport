using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.DTOs
{
    public class UserInvitationsResponseDto
    {
        public List<MemberInvitationDTOO> Invitations { get; set; }
        public int TotalData { get; set; }
    }
}
