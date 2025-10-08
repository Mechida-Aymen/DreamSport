namespace Auth.Dtos
{
    public class UserLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; } // Nullable for social logins
        public int AdminId { get; set; }
        public string? FacebookToken { get; set; } // Nullable for normal login
        public string? GoogleToken { get; set; } // Nullable for normal login
    }
}
