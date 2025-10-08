using Auth.Dtos;
using Auth.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Auth.Services
{
    public class UserService : ILoginService, IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://apigateway:8080/gateway";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetUserDto?> ValidateUserAsync(UserLogin dto)
        {
            int adminId = dto.AdminId; 

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/validate";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new { dto.Email, dto.Password }), Encoding.UTF8, "application/json");
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

            GetUserDto comingUser = await response.Content.ReadFromJsonAsync<GetUserDto>();
            return comingUser;
        }

        

        public async Task<GetUserDto?> GetUserByFacebookIdAsync(string facebookId, int AdminId, string type)
        {
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/facebook-validate/{facebookId}/{AdminId}/{type}";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl); // Change to POST
            request.Headers.Add("Tenant-ID", AdminId.ToString()); // Add AdminId to headers
            string h = "";
            request.Content = new StringContent(JsonConvert.SerializeObject(new {  }), System.Text.Encoding.UTF8, "application/json"); // Empty body with 'application/json' content type

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            GetUserDto comingUser = await response.Content.ReadFromJsonAsync<GetUserDto>();
            return comingUser;
        }

        public async Task<GetUserDto?> AddUserAsync(FacebookUserDto user, int AdminId, string type)
        {
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/Register-facebook";

            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            request.Headers.Add("Tenant-ID", AdminId.ToString()); // Add AdminId to headers
            var jsonContent = new StringContent(JsonConvert.SerializeObject(new {user.FacebookId,user.Email,user.PictureUrl,user.FirstName, user.LastName, user.Gender,type}), Encoding.UTF8, "application/json");
            request.Content = jsonContent;

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("The request was malformed or invalid.");
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            GetUserDto comingUser = await response.Content.ReadFromJsonAsync<GetUserDto>();
            return comingUser;
        }
    }
}
