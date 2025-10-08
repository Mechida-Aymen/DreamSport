using gestionReservation.Core.Interfaces;
using gestionReservation.Infrastructure.ExternServices.Extern_DTo;
using System.Net;
using System.Text;

namespace gestionReservation.Infrastructure.ExternServices
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly static string UserUrl = "http://apigateway:8080/gateway";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO> FetchUserAsync(int idUser , int adminId)
        {

            // Construct the URL (only with idUser, as AdminId will be in headers)
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/get/{idUser}";

            // Create the request with headers
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString()); // Add AdminId to headers

            Console.WriteLine($" Requesting: {requestUrl}, with Tenant-ID: {adminId}"); // Debugging Output

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string errorMsg = $" Error fetching user: {response.StatusCode}, URL: {requestUrl}";
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserDTO>();
        }
        //message broker
        public async Task<bool> ResetConteurResAnnulerAsync(int id, int adminId)
        {
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/ResetConteur/{id}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUrl); // or HttpMethod.Put
            request.Headers.Add("Tenant-ID", adminId.ToString());
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true; // Successfully reset
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false; // User not found
            }

            response.EnsureSuccessStatusCode(); // Throws an exception if status code is 4xx or 5xx
            return false;
        }
        //message broker
        public async Task<bool> CheckAndIncrementReservationAnnuleAsync(int userId, int adminId)
        {
            string requestUrl = $"{UserUrl.TrimEnd('/')}/users/check-reservation-annule/{userId}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUrl);
            request.Headers.Add("Tenant-ID", adminId.ToString());
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

    }
}
