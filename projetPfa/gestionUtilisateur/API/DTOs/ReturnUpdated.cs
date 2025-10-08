namespace gestionUtilisateur.API.DTOs
{
    public class ReturnUpdated
    {
        public int Id { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? imageUrl { get; set; }
        public int AdminId { get; set; }
        public string? bio { get; set; }
        public Dictionary<string, string> Errors { get; set; }=new Dictionary<string, string>();
    }
}
