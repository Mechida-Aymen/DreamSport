using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;



namespace gestionEmployer.Core.Models
{
    public class Personne
    {
  
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Password { get; set; }        
        public string PhoneNumber { get; set; }
    }
}
