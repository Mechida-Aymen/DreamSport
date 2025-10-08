using gestionEmployer.Core.Interfaces;
using gestionEmployer.Infrastructure.ExternServices.ExternDTOs;
using System.Text.Json;
using System.Text;

namespace gestionEmployer.Infrastructure.ExternServices
{
    public class MailService : IMailService
    {
        private readonly HttpClient _httpClient;
        private static readonly string SiteUrl = "http://apigateway:8080/Gateway/Mail/send";

        public MailService() 
        {
            _httpClient = new HttpClient();
        }

            public async Task<bool> NewEmployeeMail(EmailRequest request, int adminId)
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
