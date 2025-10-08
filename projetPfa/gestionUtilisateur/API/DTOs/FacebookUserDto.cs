namespace gestionUtilisateur.API.DTOs
{
    public class FacebookUserDto
    {
        public string FacebookId { get; set; }  // Facebook User ID
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? PictureUrl { get; set; }
        public int AdminId { get; set; }
        public string type { get; set; }
    }
}
