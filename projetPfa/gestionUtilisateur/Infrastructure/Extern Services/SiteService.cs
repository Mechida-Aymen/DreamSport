using System.Text.Json;
using System.Text;
using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;
using gestionUtilisateur.Core.Interfaces;

namespace gestionUtilisateur.Infrastructure.Extern_Services
{
    public class SiteService : ISiteService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://apigateway:8080/gateway/Site/name";

        public SiteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SiteDto> GetSiteInfosAsync(int AdminId)
        {
            string requestUrl = SiteUrl;
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
