using MovieService.Api.Models;
using MovieService.Api.Repositories;

namespace MovieService.Api.Services
{
    public class MoviesService : IMoviesService
    {
        #region Configuration
        private readonly IMoviesRepository _moviesRepository;
        private readonly MicroServiceGateway _microServiceGateway;
        public MoviesService(IMoviesRepository moviesRepository, MicroServiceGateway microServiceGateway)
        {
            _moviesRepository = moviesRepository;
            _microServiceGateway = microServiceGateway;
        }
        #endregion

        #region CreateMoviesAsync
        public async Task<Movies> CreateMoviesAsync(Movies movie)
        {
            movie.Created = DateTime.Now;
            movie.Modified = DateTime.Now;
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
        public async Task<bool> UpdateMoviesAsync(int id, Movies movie)
        {
            if (id != movie.MovieId)
                return false;
            var existingMovie = await _moviesRepository.GetMoviesByIdAsync(id);
            if (existingMovie == null)
                return false;

            existingMovie.Title = movie.Title;
            existingMovie.Genre = movie.Genre;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.Director = movie.Director;
            existingMovie.Description = movie.Description;
            existingMovie.Actor = movie.Actor;
            existingMovie.Actress = movie.Actress;
            existingMovie.Language = movie.Language;
            existingMovie.DurationMinutes = movie.DurationMinutes;
            existingMovie.PosterUrl = movie.PosterUrl;
            existingMovie.TrailerUrl = movie.TrailerUrl;

            return await _moviesRepository.UpdateMoviesAsync(existingMovie);
        }
        #endregion


    }
}
