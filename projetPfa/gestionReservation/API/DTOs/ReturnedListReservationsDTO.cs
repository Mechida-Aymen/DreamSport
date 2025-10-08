using gestionReservation.Core.Models;

namespace gestionReservation.API.DTOs
{
    public class ReturnedListReservationsDTO
    {
        public int Id { get; set; }
        public DateTime DateRes { get; set; } // La date de la réservation
        public int IdUtilisateur { get; set; }
        public int IdTerrain { get; set; }
        public int IdEmploye { get; set; }
        public int IdAdmin { get; set; }

        // Référence à la table Status
        public int? IdStatus { get; set; }  // Foreign Key vers Status
        public Status Status { get; set; }  // Le statut de la réservation (par défaut "En attente")
        public int TerrainId { get; internal set; }
    }
}
