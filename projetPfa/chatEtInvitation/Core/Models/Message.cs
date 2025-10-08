namespace chatEtInvitation.Core.Models
{
    //abstract
    public abstract class Message
    {
        public int Id { get; set; }
        public int Emetteur { get; set; }
        public DateTime when { get; set; }
        public string Contenue { get; set; }
        public ICollection<MessageStatut> Statuts { get; set; }
    }
}
