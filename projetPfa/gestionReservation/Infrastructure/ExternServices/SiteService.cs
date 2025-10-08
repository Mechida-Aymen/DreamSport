using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;
using Microsoft.Extensions.Caching.Memory;

namespace gestionReservation.Infrastructure.ExternServices
{
    public class SiteService : ISiteService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://apigateway:8080/gateway";
        public SiteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TerrainDTO> FetchTerrainAsync(int idTerrain, int adminId) // communiquer avec  gestion SIte
        {

            string requestUrl = $"{SiteUrl.TrimEnd('/')}/Terrain/by-id/{idTerrain}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers


            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<TerrainDTO>() ?? new TerrainDTO();
        }

        public async Task<SiteDto> GetSiteInfosAsync(int AdminId)
        {
            string requestUrl = SiteUrl+"/site/name";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", AdminId.ToString()); // Add AdminId to headers


            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<SiteDto>() ?? new SiteDto();
        }
    }
}
