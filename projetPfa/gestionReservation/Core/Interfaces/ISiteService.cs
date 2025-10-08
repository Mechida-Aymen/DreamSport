using gestionReservation.Infrastructure.ExternServices.Extern_DTo;

namespace gestionReservation.Core.Interfaces
{
    public interface ISiteService
    {
        Task<TerrainDTO> FetchTerrainAsync(int idTerrain, int adminId);
        Task<SiteDto> GetSiteInfosAsync(int AdminId);
    }
}
