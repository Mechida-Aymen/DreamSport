namespace gestionReservation.Core.Models
{
    public class Reservation
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
    }


    public class Status
    {
        public int Id { get; set; }
        public string Libelle { get; set; } // Exemples : "En attente", "Confirmée", "Annulée"
    }
}
