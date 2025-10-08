using System.ComponentModel.DataAnnotations;

namespace gestionEquipe.API.DTOs
{
    public class ChangerCapitaineEquipeDTO
    {
        [Required(ErrorMessage = "L'ID de l'équipe est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'ID de l'équipe doit être un entier positif.")]
        public int idEquipe {  get; set; }
        [Required(ErrorMessage = "L'ID du nouveau capitaine est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'ID du nouveau capitaine doit être un entier positif.")]
        public int idCapitain { get; set; }
    }
}
