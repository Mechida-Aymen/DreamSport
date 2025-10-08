using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.DTOs
{
    public class AddEmployeeDTO
    {
        public int AdminId { get; set; }  // Id pour identifier l'employé, peut être retourné dans l'API.
        [Required(ErrorMessage ="Nom obligatoire")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Prenom obligatoire")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "Birthday obligatoire")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Phone obligatoire")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "CIN obligatoire")]
        public string CIN { get; set; }

        [Required(ErrorMessage = "Email obligatoire")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username obligatoire")]
        public string Username { get; set; }

        public double Salaire { get; set; }
        public string? ImageUrl { get; set; }
        // Le mot de passe ne doit normalement pas être exposé dans les API, on ne l'inclut pas dans le DTO.
    }

}
