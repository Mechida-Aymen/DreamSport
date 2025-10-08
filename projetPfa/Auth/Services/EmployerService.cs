using Auth.Dtos;
using Auth.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Auth.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://apigateway:8080/gateway";
        public EmployerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetEmpLogin> LoginEmployerAsync(EmployerLoginDto userLogin)
        {
            int adminId = userLogin.AdminId;

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/employee/validate";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { userLogin.Email, userLogin.Password }), Encoding.UTF8, "application/json");
            request.Content = jsonContent;

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException("User with this Email not found");
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException("Email or Password are incorrect");
            }
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            GetEmpLogin comingUser = await response.Content.ReadFromJsonAsync<GetEmpLogin>();
            return comingUser;
            
        }
    }
}
