namespace gestionUtilisateur.API.DTOs
{
    public class ReturnForgotPasswordDTO
    {
        public string Email { get; set; }
        public int AdminId { get; set; }
        public string? error { get; set; }
    }
}
