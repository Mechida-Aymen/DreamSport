using gestionSite.Core.Models;

namespace gestionSite.Core.Interfaces.SiteInterfaces
{
    public interface ISiteRepository
    {

        Task<IEnumerable<gestionSite.Core.Models.Site>> GetAllComplexInfosAsync(int idAdmin);
        Task<gestionSite.Core.Models.Site?> AddSiteAsync(gestionSite.Core.Models.Site _site);
        Task<gestionSite.Core.Models.Site?> UpdateSiteAsync(gestionSite.Core.Models.Site _site);
        Task<Site> getSiteASync(int adminId);
        Task<Site> getByNameAsync(string name);
        Task<Site> getByEmailAsync(string email);
        Task<Site> getByPhoneAsync(string phone);
        Task<Site> getByDomaineAsync(string domaine);
    }
}
