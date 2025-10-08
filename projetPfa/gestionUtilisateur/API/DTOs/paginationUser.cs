namespace gestionUtilisateur.API.DTOs
{
    public class paginationUser
    {
        public int id {  get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phoneNumber { get; set; }
        public string? imageUrl { get; set; }
        public string? username { get; set; }
        public bool isBlocked { get; set; }
    }
}
