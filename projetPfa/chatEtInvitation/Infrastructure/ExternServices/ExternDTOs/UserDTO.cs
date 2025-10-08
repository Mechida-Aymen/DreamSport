namespace chatEtInvitation.Infrastructure.ExternServices.Extern_DTo
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Username { get; set; }
        public DateTime? Birthday { get; set; }
        public string Genre { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Tentatives { get; set; }
        public string? ImageUrl { get; set; }
        public string? Bio { get; set; }
        public int? ConteurChat { get; set; }
        public DateTime DateBlockedChat { get; set; }
        public Boolean IsChatBlocked { get; set; }
        public int ConteurReservationAnnule { get; set; }
        public DateTime DateBlockedReservation { get; set; }
        public Boolean IsReservationBlocked { get; set; }
        public int IdAdmin { get; set; }
    }
}
