
using gestionSite.Core.Models;
namespace gestionSite.Core.Interfaces.TerrainInterfaces
{
    public interface ITerrainRepository 
    {

        Task<IEnumerable<Terrain>> GetAllTerrainsByAdminAsync(int idAdmin);
        Task<Terrain?> AddTerrainAsync(Terrain terrain);
        Task<Terrain?> UpdateTerrainAsync(Terrain terrain);
        Task<Terrain?> DeleteTerrainAsync(int id);
        Task<bool> ExistsAsync(string name, int idAdmin);
        Task<Terrain?> GetTerrainByIdAsync(int id);
        Task<Terrain?> GetTerrainByIdWithStatusAsync(int id);

    }
}
