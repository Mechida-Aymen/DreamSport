namespace Auth.Model
{
    public class ValidateToken
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int AdminId { get; set; }
        public string Role {  get; set; }
        public string Nom {  get; set; }
        public string Prenom { get; set; }
        public string? ImageUrl { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? facebookId { get; set; }

    }
}
