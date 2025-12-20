using MovieService.Api.Models;

namespace MovieService.Api.Repositories
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<Movies>> GetMoviesAsync();

        Task<Movies?> GetMoviesByIdAsync(int id);
        Task<Movies> CreateMoviesAsync(Movies movie);
        Task<bool> UpdateMoviesAsync(Movies movie);
        Task<bool> DeleteMoviesAsync(int id);

    }
}
