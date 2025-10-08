using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class GetEmployeeDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string PhoneNumber { get; set; }
        public string CIN { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public string Username { get; set; }
        public double Salaire { get; set; }
        public string? imageUrl { get; set; }
        public int AdminId { get; set; }
    }
}
