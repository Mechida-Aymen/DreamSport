using gestionSite.Core.Interfaces.AnnoncesInterfaces;
using gestionSite.Core.Models;
using gestionSite.Infrastructure.Data.Repositories;


namespace gestionSite.Core.Services
{
    public class AnnoncesServices : IAnnoncesService
    {
        private readonly IAnnoncesRepository _annoncesRepository;

        public AnnoncesServices(IAnnoncesRepository annoncesRepository)
        {
            _annoncesRepository = annoncesRepository;
        }

        public async Task<IEnumerable<Annonces>> GetAnnoncesByAdminAsync(int adminId)
        {
            // Business logic can be added here (e.g., validations)
            return await _annoncesRepository.GetAllComplexAnnoncesAsync(adminId);
        }

        public async Task<Annonces?> AddAnnoncesAsync(Annonces annonces)
        {
            // Add any necessary validation or preprocessing here
            var result = await _annoncesRepository.AddAnnoncesAsync(annonces);
            return result;
        }

        public async Task<Annonces?> DeleteAnnoncesAsync(int annoncesId)
        {

            // Optionally check if the Annonces exist before deleting
            var existingAnnonces = await _annoncesRepository.DeleteAnnoncesAsync(annoncesId);
            if (existingAnnonces == null)
            {
                // Log or handle "not found" scenario
                return null;
            }

            return existingAnnonces;
        }

       
    }
}

