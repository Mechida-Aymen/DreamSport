using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;

namespace gestionUtilisateur.Core.Interfaces
{
    public interface ISiteService
    {
        Task<SiteDto> GetSiteInfosAsync(int AdminId);
    }
}
