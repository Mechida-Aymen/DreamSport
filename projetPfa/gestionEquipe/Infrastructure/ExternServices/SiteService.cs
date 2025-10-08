using gestionEquipe.Infrastructure.ExternServices.ExternDTO;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using gestionEquipe.Core.Interfaces;

namespace gestionEquipe.Infrastructure.ExternServices
{
    public class SiteService : ISiteService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "SportCategories";

        public SiteService(HttpClient httpClient, IMemoryCache cacheService)
        {
            _httpClient = httpClient;
            _cache = cacheService;
        }

        public async Task<List<SportCategorieDTO>> GetSportsAsync(int adminId)
        {
            // Check if data is in cache
            if (_cache.TryGetValue(CacheKey, out List<SportCategorieDTO> cachedData))
            {
                return cachedData;
            }

            // Ila kan lcashe khawi
            var result = await FetchSportsAsync(adminId);

            // Cache the data for 5 minutes
            _cache.Set(CacheKey, result, TimeSpan.FromMinutes(60));
            Console.WriteLine(result);
            return result;
        }

        private async Task<List<SportCategorieDTO>> FetchSportsAsync(int adminId)
        {
           
            string requestUrl = "http://apigateway:8080/gateway/SportCategorie/execute";

            // Create request message for GET
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            // Send the request
            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            // Deserialize the response content
            return await response.Content.ReadFromJsonAsync<List<SportCategorieDTO>>() ?? new List<SportCategorieDTO>();
        }

    }
}
