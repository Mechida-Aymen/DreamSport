namespace gestionReservation.API.DTOs
{
    public class UpdateStatusDTO
    {
        
        public int Id { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }
    }
}
