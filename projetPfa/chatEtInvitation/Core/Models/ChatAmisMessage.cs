namespace chatEtInvitation.Core.Models
{
    public class ChatAmisMessage : Message
    {
        public int ChatAmisId { get; set; }
        public AmisChat _AmisChat { get; set; }
    }
}
