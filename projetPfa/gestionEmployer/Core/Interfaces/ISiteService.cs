using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;

namespace gestionEmployer.Core.Interfaces
{
    public interface ISiteService
    {
        Task<SiteDto> GetSiteInfosAsync(int AdminId);
    }
}
