namespace gestionReservation.Infrastructure.ExternServices.Extern_DTo
{
    public class TerrainDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int TerrainStatusId { get; set; }
        public int IdAdmin { get; set; }
        public int IdSport_Categorie { get; set; }

        public Sport_Categorie Sport_Categorie { get; set; }
        public TerrainStatus terrainStatus { get; set; }
    }
    public class TerrainStatus
    {
        public int Id { get; set; }
        public string? Libelle { get; set; }
    }
    public class Sport_Categorie
    {
    }
}
