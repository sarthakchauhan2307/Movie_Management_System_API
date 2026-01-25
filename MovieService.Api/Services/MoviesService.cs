using MovieService.Api.DTos;
using MovieService.Api.Helper;
using MovieService.Api.Models;
using MovieService.Api.Repositories;

namespace MovieService.Api.Services
{
    public class MoviesService : IMoviesService
    {
        #region Configuration
        private readonly IMoviesRepository _moviesRepository;
        private readonly MicroServiceGateway _microServiceGateway;
        private readonly ImageHelper _imageHelper;
        public MoviesService(IMoviesRepository moviesRepository, MicroServiceGateway microServiceGateway , ImageHelper imageHelper)
        {
            _moviesRepository = moviesRepository;
            _microServiceGateway = microServiceGateway;
            _imageHelper = imageHelper;
        }
        #endregion

        #region CreateMoviesAsync
        public async Task<Movies> CreateMoviesAsync(MovieCreateUpdateDto dto)
        {
            string? posterUrl = null;

            if (dto.File != null)
            {
                posterUrl = _imageHelper.SaveImage(dto.File, "posters/movies");
            }

            var movie = new Movies
            {
                Title = dto.Title,
                Genre = dto.Genre,
                ReleaseDate = dto.ReleaseDate,
                Director = dto.Director,
                Description = dto.Description,
                Actor = dto.Actor,
                Actress = dto.Actress,
                Language = dto.Language,
                DurationMinutes = dto.DurationMinutes,
                TrailerUrl = dto.TrailerUrl,
                PosterUrl = posterUrl,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            return await _moviesRepository.CreateMoviesAsync(movie);
        }
        #endregion

        #region DeleteMoviesAsync
        public async Task<bool> DeleteMoviesAsync(int id)
        {
            var hasShow = await _microServiceGateway.HasShow(id);
            if (hasShow)
                throw new InvalidOperationException("Cannot delete movie with active shows.");
            var hasBooking = await _microServiceGateway.HasBooking(id);
            if (hasBooking)
                throw new InvalidOperationException("Cannot delete movie with active bookings.");
            return await _moviesRepository.DeleteMoviesAsync(id);
        }
        #endregion

        #region GetMoviesAsync
        public async Task<IEnumerable<Movies>> GetMoviesAsync()
        {
            return await _moviesRepository.GetMoviesAsync();
        }
        #endregion

        #region GetMoviesByIdAsync
        public async Task<Movies?> GetMoviesByIdAsync(int id)
        {
            return await _moviesRepository.GetMoviesByIdAsync(id);
        }
        #endregion

        #region UpdateMoviesAsync
        public async Task<bool> UpdateMoviesAsync(int id, MovieCreateUpdateDto dto)
        {
            var movie = await _moviesRepository.GetMoviesByIdAsync(id);
            if (movie == null)
                return false;

            if (dto.File != null)
            {
                movie.PosterUrl = _imageHelper.SaveImage(dto.File, "posters/movies");
            }


            movie.Title = dto.Title;
            movie.Genre = dto.Genre;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Director = dto.Director;
            movie.Description = dto.Description;
            movie.Actor = dto.Actor;
            movie.Actress = dto.Actress;
            movie.Language = dto.Language;
            movie.DurationMinutes = dto.DurationMinutes;
            movie.TrailerUrl = dto.TrailerUrl;
            movie.Modified = DateTime.Now;

            return await _moviesRepository.UpdateMoviesAsync(movie);
        }

        #endregion


    }
}
