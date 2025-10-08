using gestionSite.Core.Models;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;

using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
namespace gestionSite.Infrastructure.Data.Repositories
{
    public class TerrainStatusRepository : ITerrainStatusRepository
    {

        public readonly AppDbContext _context;

        public TerrainStatusRepository (AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TerrainStatus>> GetAllTerrainStatusAsync()
        {
            return await _context.TerrainStatuses.ToListAsync();
        }

        public async Task<TerrainStatus?> AddTerrainStatusAsync(TerrainStatus _terrainStatus)
        {
            try
            {
                var result = await _context.TerrainStatuses.AddAsync(_terrainStatus);
                await _context.SaveChangesAsync();

                return result.Entity;
            }
            catch
            {
                // Handle exceptions as needed (e.g., log the error)
                return null;
            }
        }

        public async Task<TerrainStatus?> DeleteTerrainStatusAsync(int id)
        {
            var _terrainStatuses = await _context.TerrainStatuses.FindAsync(id);
            if (_terrainStatuses == null)
            {
                return null;
            }

            _context.TerrainStatuses.Remove(_terrainStatuses);
            await _context.SaveChangesAsync();
            return _terrainStatuses;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.TerrainStatuses.AnyAsync(t => t.Libelle == name);
        }

        
        public async Task<TerrainStatus?> ExistsByIdAsync(int id)
        {
            return await _context.TerrainStatuses.FindAsync(id);
        }

    }
}
