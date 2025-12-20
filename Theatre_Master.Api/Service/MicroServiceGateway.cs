namespace TheatreMasterService.Api.Service
{
    public class MicroServiceGateway
    {
        #region Configuration Keys
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public MicroServiceGateway(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        #endregion

        #region HasBookingByShow
        // Check if show has any active bookings
        public async Task<bool> HasBookingByShow(int showId)
        {
            var baseUrl = _configuration["MicroServiceUrls:BookingService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("BookingService URL not configured");
            var url = $"{baseUrl}/api/booking/GetBookingsByShow/show/{showId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var bookings = await response.Content
                                    .ReadFromJsonAsync<List<object>>();

                return bookings != null && bookings.Any();
            }
            return false;
        }
        #endregion


    }
}
