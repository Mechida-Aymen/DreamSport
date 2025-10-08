using gestionSite.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace gestionSite.Core.Interfaces.AnnoncesInterfaces
{
    public interface IAnnoncesRepository
    {
        Task<IEnumerable<gestionSite.Core.Models.Annonces>> GetAllComplexAnnoncesAsync(int idAdmin);
        Task<gestionSite.Core.Models.Annonces?> AddAnnoncesAsync(gestionSite.Core.Models.Annonces _annonces);
        Task<gestionSite.Core.Models.Annonces?> DeleteAnnoncesAsync(int id);
       



    }
}
