namespace chatEtInvitation.Core.Models
{
    public class Statut
    {
        public int Id { get; set; }
        public string libelle { get; set; }
        public ICollection<MessageStatut> MessageStatuts { get; set; }
    }
}
