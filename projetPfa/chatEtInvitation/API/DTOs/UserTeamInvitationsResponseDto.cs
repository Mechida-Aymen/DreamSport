namespace chatEtInvitation.API.DTOs
{
    public class UserTeamInvitationsResponseDto
    {
        public List<MemberTeamInvitationDTOO> Invitations { get; set; }
        public int TotalData { get; set; }
    }
}
