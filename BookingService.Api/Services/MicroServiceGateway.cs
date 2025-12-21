namespace BookingService.Api.Services
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

        #region ShowExists
        public async Task<bool> GetShow(int showId)
        {
            var baseUrl = _configuration["MicroServiceUrls:TheatreMasterService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("TheatreMasterService URL not configured");
            var url = $"{baseUrl}/api/show/GetShowById/{showId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var exists = await response.Content
                                    .ReadFromJsonAsync<bool>();
                return exists;
            }
            return false;
        }
        #endregion

        #region UserExists
        public async Task<bool> UserExists(int userId)
        {
            var baseUrl = _configuration["MicroServiceUrls:UserService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("UserService URL not configured");
            var url = $"{baseUrl}/api/user/GetUserById/{userId}";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var exists = await response.Content
                                    .ReadFromJsonAsync<bool>();
                return exists;
            }
            return false;
        }
        #endregion

        

    }
}
