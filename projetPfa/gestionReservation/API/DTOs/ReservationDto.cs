namespace gestionReservation.API.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime DateRes { get; set; }
        public int IdUtilisateur { get; set; }
        public int IdTerrain { get; set; }
        public int IdEmploye { get; set; }
        public int IdAdmin { get; set; }
        public string StatusLibelle { get; set; } // Libellé du statut
    }
}
