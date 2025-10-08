using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gestionSite.Core.Interfaces.TerrainInterfaces
{
    public interface ITerrainService
    {
        Task<IEnumerable<Terrain>> GetAllTerrainsByAdminAsync(int idAdmin);
        Task<Terrain?> AddTerrainAsync(Terrain terrain);
        Task<Terrain?> UpdateTerrainAsync(Terrain terrain);
        Task<Terrain?> DeleteTerrainAsync(int id,int adminId);
        Task<bool> ExistsAsync(string name, int idAdmin);
        Task<Terrain?> GetTerrainByIdAsync(int id);
        Task<Terrain?> GetTerrainByIdWithStatusAsync(int id, int adminId);
        Task<Terrain?> UpdateTerrainStatusAsync(UpdateStatusDto dto);
    }
}
