namespace gestionUtilisateur.API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public string? Bio { get; set; }
    }
}
