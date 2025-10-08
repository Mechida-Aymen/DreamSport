using gestionEmployer.API.DTOs.EmployeeDTO;
using gestionEmployer.Core.Interfaces;
using System.Text.Json;
using System.Text;

namespace gestionEmployer.Infrastructure.ExternServices
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://apigateway:8080/gateway/Login";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UpdateTokenAsync(SendLoginEmployeeDto dto, int AdminId)
        {
            string requestUrl = SiteUrl;
            var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);

            request.Headers.Add("Tenant-ID", AdminId.ToString()); // Add AdminId to headers

            // Serialize dto to JSON and set as request body
            string jsonBody = JsonSerializer.Serialize(dto);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

        }
    }
}
