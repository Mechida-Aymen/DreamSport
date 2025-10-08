namespace chatEtInvitation.Core.Models
{
    //abstract
    public abstract class Invitation
    {
        public int Emetteur { get; set; }
        public int Recerpteur { get; set; }
        public int AdminId { get; set; }

    }
}
