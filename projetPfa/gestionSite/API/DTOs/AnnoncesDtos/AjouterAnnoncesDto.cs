using System.ComponentModel.DataAnnotations;

namespace gestionSite.API.DTOs.AnnoncesDtos
{
    public class AjouterAnnoncesDto
    {
        [Required(ErrorMessage = "")]
        public string? Description { get; set; }
        [Required]
        public DateTime LaunchedAt { get; set; }
        [Required]
        public int LifeTimeBySeconds { get; set; }
        [Required]
        public int AdminId { get; set; }
    }
}
