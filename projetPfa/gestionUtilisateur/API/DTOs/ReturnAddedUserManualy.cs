using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
    public class ReturnAddedUserManualy
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Username { get; set; }
        public DateTime? Birthday { get; set; }
        public string Genre { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public int AdminId { get; set; }

        public Dictionary<string, string> errors { get; set; } = new Dictionary<string, string>();
    }
}
