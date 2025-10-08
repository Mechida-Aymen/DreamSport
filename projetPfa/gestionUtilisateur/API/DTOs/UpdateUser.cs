using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
    public class UpdateUser
    {
        public int Id { get; set; }
        public string? lastname { get; set; }
        public string? firstname { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public string? bio { get; set; }
        public int AdminId { get; set; }
    }
}
