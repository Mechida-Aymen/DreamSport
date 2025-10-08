namespace chatEtInvitation.Core.Models
{
    public class TeamChatMessage : Message
    {
        public int TeamChatId { get; set; } 
        public TeamChat _TeamChat { get; set; }
    }
}
