using Microsoft.EntityFrameworkCore;
using MovieService.Api.Data;
using MovieService.Api.Models;

namespace MovieService.Api.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        #region Configuration
        private readonly MoviesDbContext _context;

        public MoviesRepository(MoviesDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetMovies
        public async Task<IEnumerable<Movies>> GetMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
        }
        #endregion

        #region GetMoviesByIdAsync
        public async Task<Movies?> GetMoviesByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }
        #endregion

        #region CreateMoviesAsync
        public async Task<Movies> CreateMoviesAsync(Movies movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
        }
        #endregion

        #region UpdateMoviesAsync
        public async Task<bool> UpdateMoviesAsync(Movies movie)
        {
            _context.Movies.Update(movie);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region DeleteMoviesAsync
        public async Task<bool> DeleteMoviesAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return false;
            _context.Movies.Remove(movie);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

    }
}
