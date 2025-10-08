using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
    public class UpdateUserDto
    {
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? Username { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Genre { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? PasswordConfirmed { get; set; }
        [Required(ErrorMessage = "required")]
        public int AdminId { get; set; }
        public string? bio { get; set; }
    }
}
