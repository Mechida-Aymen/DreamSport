using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Infrastructure.ExternServices.Extern_DTo;

namespace chatEtInvitation.Infrastructure.ExternServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://apigateway:8080/gateway";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> FetchUserAsync(int idUser,int adminId)
        {

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/get/{idUser}";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            Console.WriteLine($"🔍 Requesting: {requestUrl}, with Tenant-ID: {adminId}"); // Debugging Output

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = $"❌ Error fetching user: {response.StatusCode}, URL: {requestUrl}";
            }

            return await response.Content.ReadFromJsonAsync<UserDTO>() ?? new UserDTO();
        }

    }
}
