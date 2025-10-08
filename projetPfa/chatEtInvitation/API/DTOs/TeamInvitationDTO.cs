using System.ComponentModel.DataAnnotations;

namespace chatEtInvitation.API.DTOs
{
    public class TeamInvitationDTO
    {
        [Required(ErrorMessage = "L'émetteur est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'émetteur doit être un identifiant valide.")]
        public int Emetteur { get; set; }

        [Required(ErrorMessage = "Le récepteur est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "Le récepteur doit être un identifiant valide.")]
        public int Recepteur { get; set; }

        [Required(ErrorMessage = "L'ID de l'équipe (AdminId) est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'ID de l'équipe doit être un identifiant valide.")]
        public int AdminId { get; set; }
        public string? TeamName { get; set; }

        public int? idInv { get; set; }
    }
}
