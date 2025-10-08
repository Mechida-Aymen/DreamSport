namespace gestionReservation.API.DTOs
{
    public class GetReservationsDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AdminId { get; set; }

    }
}
