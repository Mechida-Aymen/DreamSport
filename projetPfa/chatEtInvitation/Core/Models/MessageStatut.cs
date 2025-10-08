namespace chatEtInvitation.Core.Models
{
    public class MessageStatut
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }

        public bool IsTeam { get; set; }
        public int StatutId { get; set; }
        public Statut Statut { get; set; }

        public int UtilisateurId { get; set; }
    }
}
