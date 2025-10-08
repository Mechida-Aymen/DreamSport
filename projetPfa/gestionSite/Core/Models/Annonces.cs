namespace gestionSite.Core.Models
{
    public class Annonces
    {
        public int Id { get; set; }         
        public string? Description { get; set; }
        public DateTime LaunchedAt { get; set; }
        public int LifeTimeBySeconds { get; set; }
        public int IdAdmin {  get; set; }
    }
}
