using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.Mappers
{
    public class MemberInvitationMapper
    {
        public static MemberInvitation AddDtoToModel(AddInvMemberDto dto)
        {
            return new MemberInvitation
            {
                AdminId = dto.AdminId,
                Emetteur = dto.Emetteur,
                Recerpteur = dto.Recerpteur,
            };
        }

        // Mapper MemberInvitation en InvitationDto
        public AddInvMemberDto ToDto(MemberInvitation invitation)
        {
            return new AddInvMemberDto
            {
                AdminId = invitation.AdminId,
                Emetteur = invitation.Emetteur,
                Recerpteur = invitation.Recerpteur,
            };
        }
    }
}
