using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.AdminDTO
{
    public class AjouterAdminDTO
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Prenom { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Login { get; set; }
    }


}