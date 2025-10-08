namespace gestionUtilisateur.API.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int AdminId { get; set; }
    }
}
