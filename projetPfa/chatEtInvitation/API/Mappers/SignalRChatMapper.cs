using chatEtInvitation.API.DTOs;
using chatEtInvitation.Core.Models;

namespace chatEtInvitation.API.Mappers
{
    public class SignalRChatMapper
    {
        public static UserInfoSignalRDTO UserToUserSignalRDTO(UserInfoDTO result)
        {
            return new UserInfoSignalRDTO
            {
                id = result.Id,
                avatar = result.Avatar,
            };
        }

        public static AmisMessageSignalRDTO AmisMessageSignalRDTO(AmisMessageDTO result)
        {
            return new AmisMessageSignalRDTO
            {
                id = result.Id,
                contenu = result.Contenu,
                dateEnvoi = result.DateEnvoi,
                statut = result.Statut,
                chatAmisId = result.chatAmisId
            };
        }
        public static SendTeamSignaRDTO TeamMessageSignalRDTO(TeamMessageDTO result)
        {
            return new SendTeamSignaRDTO
            {
                id = result.Id,
                contenu = result.Contenu,
                dateEnvoi = result.DateEnvoi,
                statut = result.Statut,
                emetteur=SignalRChatMapper.UserToUserSignalRDTO(result.Emetteur),
                teamId=result.teamId,
                chatTeamId=result.chatTeamId
            };
        }
    }
}
