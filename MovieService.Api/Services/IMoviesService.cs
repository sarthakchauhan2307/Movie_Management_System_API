using MovieService.Api.Models;

namespace MovieService.Api.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movies>> GetMoviesAsync();
        Task<Movies?> GetMoviesByIdAsync(int id);
        Task<Movies> CreateMoviesAsync(Movies movie);
        Task<bool> UpdateMoviesAsync(int id,Movies movie);
        Task<bool> DeleteMoviesAsync(int id);

    }
}
