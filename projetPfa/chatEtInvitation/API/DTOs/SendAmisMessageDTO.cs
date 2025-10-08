namespace chatEtInvitation.API.DTOs
{
    public class SendAmisMessageDTO
    {
        public int ChatAmisId { get; set; }
        public int EmetteurId { get; set; }
        public string Contenu { get; set; }
        public int AdminId { get; set; }
    }
    public class AmisMessageDTO
    {
        public int Id { get; set; }
        public string Contenu { get; set; }
        public DateTime DateEnvoi { get; set; }
        public UserInfoDTO Emetteur { get; set; }
        public string Statut { get; set; }
        public int? RecepteurId { get; set; }
        public int chatAmisId { get; set; }

    }

    public class AmisMessageSignalRDTO
    {
        public int id { get; set; }
        public string contenu { get; set; }
        public DateTime dateEnvoi { get; set; }
        public UserInfoSignalRDTO emetteur { get; set; }
        public string statut { get; set; }
        public int? RecepteurId { get; set; }
        public int chatAmisId { get; set; }

    }

    public class UserInfoSignalRDTO
    {
        public int id { get; set; }
        public string nomComplet { get; set; }
        public string avatar { get; set; }
    }
}
