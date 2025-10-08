namespace gestionSite.API.DTOs.TerrainDtos
{
    public class UpdateTerrainDto
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int TerrainStatusId { get; set; }
        public int AdminId { get; set; }
    }
}
