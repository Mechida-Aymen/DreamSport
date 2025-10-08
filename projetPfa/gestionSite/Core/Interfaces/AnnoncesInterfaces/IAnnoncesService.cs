 using gestionSite.Core.Models;

namespace gestionSite.Core.Interfaces.FAQ
    {
        public interface IAnnoncesService
        {
            Task<IEnumerable<Annonces>> GetAnnoncesByAdminAsync(int adminId);
            Task<Annonces?> AddAnnoncesAsync(Annonces annonces);
            Task<Annonces?> DeleteAnnoncesAsync(int annonceId);
         
        }
    }


