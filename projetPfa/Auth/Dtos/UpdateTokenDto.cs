namespace Auth.Dtos
{
    public class UpdateTokenDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Image { get; set; }
        public int AdminId { get; set; }
        public string Role { get; set; }
    }
}
