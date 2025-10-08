namespace chatEtInvitation.API.DTOs
{
    public class CreateChatDto
    {
        public int Member1Id { get; set; }
        public int Member2Id { get; set; }
    }

    public class ChatDto
    {
        public int Id { get; set; }
        public int Member1Id { get; set; }
        public int Member2Id { get; set; }
        public int AdminId { get; set; }
    }

}
