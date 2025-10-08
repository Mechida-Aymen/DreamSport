namespace chatEtInvitation.API.DTOs
{
    public class UserUnblockDTO
    {
        public int userIdToUnblock { get; set; }
        public int currentUserId { get; set; }
        public int AdminId { get; set; }
    }
}
