using System.ComponentModel.DataAnnotations;

namespace gestionEmployer.API.DTOs.AdminDTO
{
    public class ChangeAdminPasswordDto
    {
        [Required]
        public int AdminId { get; set; }


        [Required]
        public string OldPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
