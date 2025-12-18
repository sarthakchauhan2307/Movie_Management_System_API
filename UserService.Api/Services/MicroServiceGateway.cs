using System.Net.Http.Json;

namespace UserService.Api.Services
{
    public class MicroServiceGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public MicroServiceGateway(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> HasBooking(int userId)
        {
            var baseUrl = _config["MicroserviceUrls:BookingService"];

            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("BookingService URL is not configured.");

            var url = $"{baseUrl}/api/booking/user/{userId}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return false;

            var bookings = await response.Content.ReadFromJsonAsync<List<object>>();
            return bookings != null && bookings.Count > 0;
        }
    }
}
