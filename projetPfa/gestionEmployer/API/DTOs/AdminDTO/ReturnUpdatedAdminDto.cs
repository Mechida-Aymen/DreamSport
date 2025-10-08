namespace gestionEmployer.API.DTOs.AdminDTO
{
    public class ReturnUpdatedAdminDto
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

    }
}
