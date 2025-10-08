using gestionSite.Core.Models;

namespace gestionSite.Core.Interfaces.SportInterfaces
{
    public interface ISportRepository
    {
        Task<IEnumerable<Sport_Categorie>> GetSportsAsync();

    }
}
