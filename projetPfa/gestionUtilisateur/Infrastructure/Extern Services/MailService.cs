using gestionUtilisateur.Infrastructure.Extern_Services.Extern_DTOs;
using System.Text.Json;
using System.Text;
using gestionUtilisateur.Core.Interfaces;

namespace gestionUtilisateur.Infrastructure.Extern_Services
{
    public class MailService : IMailService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://apigateway:8080/gateway/Mail/send";

        public MailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> MailRecoverkey(EmailRequest request, int adminId)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, SiteUrl);
            req.Headers.Add("Tenant-ID", adminId.ToString());

            // Create JSON content and set it as the request body
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            req.Content = jsonContent;


            var response = await _httpClient.SendAsync(req);
            return response.IsSuccessStatusCode;
        }
    }
}
