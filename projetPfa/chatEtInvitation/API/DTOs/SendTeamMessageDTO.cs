namespace chatEtInvitation.API.DTOs
{
    public class SendTeamMessageDTO
    {
        public int TeamId { get; set; }
        public int EmetteurId { get; set; }
        public string Contenu { get; set; }
        public int AdminId { get; set; }
        public int? chatTeamId { get; set; }

    }
}
