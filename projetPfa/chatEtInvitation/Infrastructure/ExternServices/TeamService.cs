using chatEtInvitation.Core.Interfaces.IExternServices;
using chatEtInvitation.Infrastructure.ExternServices.ExternDTOs;

namespace chatEtInvitation.Infrastructure.ExternServices
{
    public class TeamService : ITeamService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://apigateway:8080/gateway";

        public TeamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> FetchMembersAsync(int TeamId , int adminId)
        {

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/equipe/{TeamId}";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<List<int>>() ?? null;
        }

        public async Task<teamDTO> FetchTeamAsync(int teamId, int adminId)
        {

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/equipe/get/{teamId}";

            // Create the request with headers
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            Console.WriteLine($"🔍 Requesting: {requestUrl}, with Tenant-ID: {adminId}"); // Debugging Output

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = $"❌ Error fetching user: {response.StatusCode}, URL: {requestUrl}";
            }

            return await response.Content.ReadFromJsonAsync<teamDTO>() ?? new teamDTO();
        }


    }
}
