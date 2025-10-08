using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.Mappers
{
    public class TeamInvMapper
    {
        public static TeamInvitation SendInvToModel(TeamInvitationDTO teamInvitationDTO)
        {
            return new TeamInvitation
            {
                AdminId = teamInvitationDTO.AdminId,
                Emetteur = teamInvitationDTO.Emetteur,
                Recerpteur = teamInvitationDTO.Recepteur,
            };
        }
    }
}
