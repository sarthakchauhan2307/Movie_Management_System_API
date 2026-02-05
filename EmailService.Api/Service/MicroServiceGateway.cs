using EmailService.Api.Model;

namespace EmailService.Api.Services
{
    public class MicroServiceGateway
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public MicroServiceGateway(HttpClient http,IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<byte[]> DownloadTicketAsync(int bookingId)
        {
            var baseUrl = _configuration["MicroServiceUrls:BookingService"];
            var response =
                await _http.GetAsync($"{baseUrl}/api/booking/DownloadTicket/download/{bookingId}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<string> GetUserEmailAsync(int userId)
        {
            var baseUrl = _configuration["MicroServiceUrls:UserService"];
            var url = $"{baseUrl}/api/user/GetUserById/{userId}";

            var user = await _http.GetFromJsonAsync<UserDto>(url);
            return user!.Email;
        }
    }
}
