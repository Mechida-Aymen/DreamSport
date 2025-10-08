using gestionSite.Core.Interfaces.TerrainStatutsInterfaces;
using gestionSite.Core.Models;

namespace gestionSite.Core.Services
{
    public class TerrainStatusService : ITerrainStatusService
    {
        private readonly ITerrainStatusRepository _terrainStatusRepository;

        public TerrainStatusService (ITerrainStatusRepository _ITerrainStatusRepository)
        {
            _terrainStatusRepository = _ITerrainStatusRepository;

        }

        public async Task<IEnumerable<TerrainStatus>> GetAllTerrainStatusAsync()
        {
            return await _terrainStatusRepository.GetAllTerrainStatusAsync();

        }

        public async Task<TerrainStatus?> AddTerrainStatusAsync(TerrainStatus _terrainStatus)
        {
            // Add any necessary validation or preprocessing here
            if (await _terrainStatusRepository.ExistsAsync(_terrainStatus.Libelle))
            {
                throw new InvalidOperationException("Status already exists already exists");
            }

            return await _terrainStatusRepository.AddTerrainStatusAsync(_terrainStatus);
        }
        public async Task<TerrainStatus?> DeleteTerrainStatusAsync(int id)
        {
            var existingStatus = await _terrainStatusRepository.DeleteTerrainStatusAsync(id);
            if (existingStatus == null)
            {
                // Log or handle "not found" scenario
                return null;
            }

            return existingStatus;
        }
    }
}
