using BookingService.Api.DTo;
using System.Text.Json;

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

        //  ADD THIS (FOR RABBITMQ)
        #region GetShowDetails
        public async Task<BookingShow> GetShowDetailsAsync(int showId)
        {
            var baseUrl = _configuration["MicroServiceUrls:TheatreMasterService"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("TheatreMasterService URL not configured");

            var url = $"{baseUrl}/api/Show/GetShowById/{showId}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to fetch show details");

            var showDetails =
             await response.Content.ReadFromJsonAsync<BookingShow>(
                 new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 }
            );

            return showDetails!;
        }
        #endregion

        #region GetMovieAsync
        public async Task<MovieDto> GetMovieAsync(int movieId)
        {
            var baseUrl = _configuration["MicroServiceUrls:Movieservice"];
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("MovieService URL not configured");

            var url = $"{baseUrl}/api/Movies/GetMovieById/{movieId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to fetch movie");

            var movie = await response.Content.ReadFromJsonAsync<MovieDto>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return movie!;
        }
        #endregion


    }
}
