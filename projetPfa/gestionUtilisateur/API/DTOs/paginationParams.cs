namespace gestionUtilisateur.API.DTOs
{
    public class paginationParams
    {
        public int skip {  get; set; } = 0;
        public int limit { get; set; } = 10;
        public bool? isBlocked { get; set; } = null;
        public int AdminId { get; set; }
        public string? searchTerm { get; set; }
    }
}
