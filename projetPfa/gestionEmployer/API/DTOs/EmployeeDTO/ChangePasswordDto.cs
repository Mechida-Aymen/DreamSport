using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.EmployeeDTO
{
    // ChangePasswordDto.cs
    public class ChangePasswordDto
    {
        [Required]
        public int AdminId { get; set; }

        [Required]
        public int EmployerId { get; set; }

        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
