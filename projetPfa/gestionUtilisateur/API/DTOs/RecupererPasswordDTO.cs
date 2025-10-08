using System.ComponentModel.DataAnnotations;

namespace gestionUtilisateur.API.DTOs
{
    public class RecupererPasswordDTO
    {
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "L'adresse email n'est pas valide.")]
        public string Email { get; set; } 
        
        public int AdminId { get; set; }

        


    }
}
