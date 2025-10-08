
using gestionSite.Core.Interfaces.TerrainInterfaces;
using Microsoft.EntityFrameworkCore;
using gestionSite.Core.Models;
using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
using gestionSite.Core.Interfaces.CasheInterfaces;

namespace gestionSite.Core.Services
{
    public class TerrainService : ITerrainService
    {
        private readonly ICacheService _cacheService;
        private readonly ITerrainRepository _terrainRepository;
        private readonly ITerrainStatusRepository _terrainStatusRepository;

        public TerrainService(ICacheService cacheService, ITerrainRepository terrainRepository, ITerrainStatusRepository terrainStatusRepository)
        {
            _cacheService = cacheService;
            _terrainRepository = terrainRepository;
            _terrainStatusRepository = terrainStatusRepository;
        }

        // Récupérer tous les terrains associés à un administrateur
        public async Task<IEnumerable<Terrain>> GetAllTerrainsByAdminAsync(int idAdmin)
        {
            string cacheKey = $"courts:all:{idAdmin}";

            IEnumerable<Terrain> courts = await _cacheService.GetAsync<IEnumerable<Terrain>>(cacheKey);
            if (courts == null)
            {
                // If not found in cache, fetch from DB
                courts = await _terrainRepository.GetAllTerrainsByAdminAsync(idAdmin);
                if (courts != null)
                {
                    // Cache the result in both L1 and L2
                    await _cacheService.SetAsync(cacheKey, courts, TimeSpan.FromMinutes(5));  // Set TTL as needed
                }      
            }

            return courts;
        }

        // Ajouter un terrain
        public async Task<Terrain?> AddTerrainAsync(Terrain terrain)
        {
            // Logique de validation avant l'ajout
            if (await _terrainRepository.ExistsAsync(terrain.Title, terrain.IdAdmin))
            {
                throw new InvalidOperationException("A terrain with this title already exists.");
            }
            

            return await _terrainRepository.AddTerrainAsync(terrain);
        }

        // Mettre à jour un terrain
        public async Task<Terrain?> UpdateTerrainAsync(Terrain terrain)
        {
            // Vérifier que le terrain existe avant la mise à jour
            var existingTerrain = await this.GetTerrainByIdAsync(terrain.Id);
            if (existingTerrain == null)
            {
                throw new KeyNotFoundException("Terrain not found.");
            }

            Terrain court = await _terrainRepository.UpdateTerrainAsync(terrain);

            if (court != null)
            {
                await _cacheService.RemoveAsync($"court:{terrain.Id}");  // Remove from both caches
                await _cacheService.RemoveAsync($"courts:all:{terrain.IdAdmin}");
            }
            return court;
        }

        // Supprimer un terrain
        public async Task<Terrain?> DeleteTerrainAsync(int id, int adminId)
        {
            var existingTerrain = await _terrainRepository.GetTerrainByIdAsync(id);
            if (existingTerrain == null)
            {
                return null; // Terrain non trouvé
            }

            Terrain court = await _terrainRepository.DeleteTerrainAsync(id);
            if (court != null)
            {
                await _cacheService.RemoveAsync($"court:{id}");  // Remove from both caches
                await _cacheService.RemoveAsync($"courts:all:{adminId}");
            }
            return court;
        }

        // Vérifier si un terrain existe par son titre pour un administrateur donné
        public async Task<bool> ExistsAsync(string title, int idAdmin)
        {
            return await _terrainRepository.ExistsAsync(title, idAdmin);
        }

        // Récupérer un terrain par son ID
        public async Task<Terrain?> GetTerrainByIdAsync(int id)
        {
            string cacheKey = $"court:{id}";

            Terrain court = await _cacheService.GetAsync<Terrain>(cacheKey);
            if (court == null)
            {
                court = await _terrainRepository.GetTerrainByIdAsync(id);
                if (court != null)
                {
                    await _cacheService.SetAsync(cacheKey, court, TimeSpan.FromMinutes(5));
                }
            }
            return court;
        }

        public async Task<Terrain?> GetTerrainByIdWithStatusAsync(int id, int adminId)
        {
            string cacheKey = $"court:{id}";

            Terrain court = await _cacheService.GetAsync<Terrain>(cacheKey);
            if (court == null)
            {
                court = await _terrainRepository.GetTerrainByIdWithStatusAsync(id);
                if (court != null)
                {
                    await _cacheService.SetAsync(cacheKey, court, TimeSpan.FromMinutes(5));
                }
            }

            return court;
        }

        public async Task<Terrain?> UpdateTerrainStatusAsync(UpdateStatusDto dto)
        {
            Terrain terrain = await _terrainRepository.GetTerrainByIdAsync(dto.Id);
            if (terrain == null)
            {
                throw new KeyNotFoundException("The court not found ");
            }
            if (await _terrainStatusRepository.ExistsByIdAsync(dto.statusId) == null)
            {
                throw new KeyNotFoundException("The Status you provided not found ");
            }
            terrain.TerrainStatusId = dto.statusId;
            Terrain court = await _terrainRepository.UpdateTerrainAsync(terrain);
            if (court != null)
            {
                await _cacheService.RemoveAsync($"court:{dto.Id}");  // Remove from both caches
                await _cacheService.RemoveAsync($"courts:all:{dto.AdminId}");
            }
            return court;
        }
    }
}
