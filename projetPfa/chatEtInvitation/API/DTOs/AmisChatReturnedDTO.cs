namespace chatEtInvitation.API.DTOs
{
    public class AmisChatReturnedDTO
    {
        public int Id { get; set; }
        public string AmiName { get; set; } 
        public int? idMember { get; set; }
        public string LastMessage { get; set; }
        public DateTime Date { get; set; }
        public int? UnreadCount { get; set; }
        public string Statut { get; set; }
        public string Avatar { get; set; }
        
    }
}
