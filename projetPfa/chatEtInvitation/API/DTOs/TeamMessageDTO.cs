using chatEtInvitation.Infrastructure.ExternServices.ExternDTOs;

namespace chatEtInvitation.API.DTOs
{
    public class TeamMessageDTO
    {
        public int Id { get; set; }
        public string Contenu { get; set; }
        public DateTime DateEnvoi { get; set; }
        public UserInfoDTO Emetteur { get; set; }
        public string Statut { get; set; }
        public List<MembreDto>? TeamMemberIds { get; set; }
        public int chatTeamId;
        public int? teamId;

    }

    public class UserInfoDTO
    {
        public int Id { get; set; }
        public string NomComplet { get; set; }
        public string Avatar { get; set; }
    }
}
