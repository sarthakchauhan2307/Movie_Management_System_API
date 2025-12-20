using System.Net.Http.Json;
namespace MovieService.Api.Services
{
    public class MicroServiceGateway
    {
        #region  Configuration Keys
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public MicroServiceGateway(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        #endregion

        #region HasShow
        //  Check if movie has any active shows
        public async Task<bool> HasShow(int movieId)//async nai mukto
        {
            var baseUrl = _configuration["MicroserviceUrls:TheatreMasterService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("TheatreMasterService URL not configured");

            var url = $"{baseUrl}/api/Show/GetShowsByMovieId/movie/{movieId}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(json))
                return false;

            var shows = System.Text.Json.JsonSerializer.Deserialize<List<object>>(json);
            return shows != null && shows.Count > 0;
        }

        #endregion

        #region HasBooking
        // Check if movie has any active bookings
        public async Task<bool> HasBooking(int movieId)
        {
            var baseUrl = _configuration["MicroServiceUrls:BookingService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("BookingService URL not configured");
            var url = $"{baseUrl}/api/booking/movie/{movieId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var bookings = await response.Content.ReadFromJsonAsync<bool>();
                return bookings;
            }
            return false;
        }
        #endregion

       

    }
}
