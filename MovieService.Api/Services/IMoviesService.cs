using MovieService.Api.Models;
using MovieService.Api.DTos;

namespace MovieService.Api.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movies>> GetMoviesAsync();
        Task<Movies?> GetMoviesByIdAsync(int id);

        Task<Movies> CreateMoviesAsync(MovieCreateUpdateDto dto);
        Task<bool> UpdateMoviesAsync(int id, MovieCreateUpdateDto dto);

        Task<bool> DeleteMoviesAsync(int id);
    }
}
