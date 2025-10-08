using gestionSite.API.DTOs.SiteDtos;
using gestionSite.API.DTOs.TerrainDtos;
using gestionSite.Core.Models;

namespace gestionSite.Core.Interfaces.SiteInterfaces
{
    public interface ISiteService
    {
        Task<IEnumerable<gestionSite.Core.Models.Site>> GetSiteByAdminAsync(int adminId);
        Task<gestionSite.Core.Models.Site?> AddSiteAsync(int adminId);
        Task<ReturnUpdatedSiteDto?> UpdateSiteAsync(Site updatingSite);
        Task<Site> GetSiteASync(int adminId);
    }
}
