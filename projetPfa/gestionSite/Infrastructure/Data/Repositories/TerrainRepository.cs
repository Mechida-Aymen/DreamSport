using gestionSite.Core.Interfaces.TerrainInterfaces;


using gestionSite.Core.Models;

namespace gestionSite.Infrastructure.Repositories
{
    public class TerrainRepository : ITerrainRepository
    {
        private readonly AppDbContext _context;

        public TerrainRepository(AppDbContext context)
        {
            _context = context;
        }

        // Récupérer tous les terrains associés à un administrateur
        public async Task<IEnumerable<Terrain>> GetAllTerrainsByAdminAsync(int idAdmin)
        {
            return await _context.Terrains
                .Where(t => t.IdAdmin == idAdmin)
                .ToListAsync();
             
        }

        // Ajouter un terrain
        public async Task<Terrain?> AddTerrainAsync(Terrain terrain)
        {
            var __terrainStatus = await _context.TerrainStatuses
                                  .FirstOrDefaultAsync(ts => ts.Id == terrain.TerrainStatusId);
            if (__terrainStatus != null)
            {
                try
                {
                terrain.TerrainStatusId = __terrainStatus.Id;
                await _context.Terrains.AddAsync(terrain);
                await _context.SaveChangesAsync();
                return terrain;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        // Mettre à jour un terrain
        public async Task<Terrain?> UpdateTerrainAsync(Terrain terrain)
        {
            var existingTerrain = await _context.Terrains
                .FirstOrDefaultAsync(t => t.Id == terrain.Id);

            if (existingTerrain == null) return null;

            existingTerrain.Title = terrain.Title;
            existingTerrain.Description = terrain.Description;
            existingTerrain.Image = terrain.Image;
            existingTerrain.TerrainStatusId = terrain.TerrainStatusId;

            await _context.SaveChangesAsync();
            return existingTerrain;
        }

        // Supprimer un terrain
        public async Task<Terrain?> DeleteTerrainAsync(int id)
        {
            var terrain = await _context.Terrains.FindAsync(id);
            if (terrain == null) return null;

            _context.Terrains.Remove(terrain);
            await _context.SaveChangesAsync();
            return terrain;
        }

        // Vérifier si un terrain existe par titre et administrateur
        public async Task<bool> ExistsAsync(string title, int idAdmin)
        {
            return await _context.Terrains
                .AnyAsync(t => t.Title == title && t.IdAdmin == idAdmin);
        }

        // Récupérer un terrain par son ID
        public async Task<Terrain?> GetTerrainByIdAsync(int id)
        {
            return await _context.Terrains.FindAsync(id);
        }

        public async Task<Terrain?> GetTerrainByIdWithStatusAsync(int id)
        {
            return await _context.Terrains
                .Include(t => t.terrainStatus)  // Load TerrainStatus
                .FirstOrDefaultAsync(t => t.Id == id);
        }

    }
}
    

