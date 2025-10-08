using gestionSite.Core.Models;
namespace gestionSite.Core.Interfaces.TerrainStatutsInterfaces
{
    public interface ITerrainStatusRepository
    {
        Task<IEnumerable<TerrainStatus>> GetAllTerrainStatusAsync();
        Task<TerrainStatus?> AddTerrainStatusAsync(TerrainStatus _terrainStatus);
        Task<TerrainStatus?> DeleteTerrainStatusAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<TerrainStatus?> ExistsByIdAsync(int id);
    }
}
