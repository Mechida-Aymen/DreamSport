using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    public class GetAllEmployeeDTO
    {
        [Required, Range(1, int.MaxValue, ErrorMessage = "L'ID doit être supérieur à 0.")]
        public int AdminId { get; set; }
    }
}
