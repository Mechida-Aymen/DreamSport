using System.ComponentModel.DataAnnotations;

namespace gestionReservation.API.DTOs
{
    public class AddReservationDto
    {
        [Required(ErrorMessage ="You need to enter the date")]
        public DateTime DateRes { get; set; }
        [Required]
        public int IdUtilisateur { get; set; }
        [Required(ErrorMessage ="You need to choice a stadium")]
        public int IdTerrain { get; set; }
        [Required]
        public int AdminId { get; set; }
    }
}
