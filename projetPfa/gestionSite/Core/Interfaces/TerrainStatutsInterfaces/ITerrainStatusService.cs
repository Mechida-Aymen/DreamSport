using gestionSite.Core.Models;
namespace gestionSite.Core.Interfaces.TerrainStatutsInterfaces
{
    public interface ITerrainStatusService
    {
        Task<IEnumerable<TerrainStatus>> GetAllTerrainStatusAsync();
        Task<TerrainStatus?> AddTerrainStatusAsync(TerrainStatus _terrainStatus);
        Task<TerrainStatus?> DeleteTerrainStatusAsync(int id);
    }
}
