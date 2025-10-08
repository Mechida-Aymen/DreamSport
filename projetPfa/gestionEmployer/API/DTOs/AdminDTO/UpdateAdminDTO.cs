namespace gestionEmployer.API.DTOs.AdminDTO
{
    public class UpdateAdminDTO
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public int AdminId { get; set; }

    }
}
