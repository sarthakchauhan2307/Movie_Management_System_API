using Dapper;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Data;
using MovieService.Api.Models;
using System.Data;

namespace MovieService.Api.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        #region Configuration
        private readonly DapperContext _context;

        public MoviesRepository(DapperContext context)
        {
            _context = context;
        }
        #endregion

        #region GetMovies
        public async Task<IEnumerable<Movies>> GetMoviesAsync()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<Movies>(
                "SP_GetMovie",
                commandType: CommandType.StoredProcedure
               );
        }
        #endregion

        #region GetMoviesByIdAsync
        public async Task<Movies?> GetMoviesByIdAsync(int id)
        {
           using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Movies>(
                "SP_GetMovieById",
                new { MovieId = id },
                commandType: CommandType.StoredProcedure
               );
        }
        #endregion

        #region CreateMoviesAsync
        public async Task<Movies> CreateMoviesAsync(Movies movie)
        {
          using var connection = _context.CreateConnection();
            var movieId = await connection.QuerySingleAsync<int>(
                "SP_CreateMovie",
                new
                {
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Director,
                    movie.Description,
                    movie.Actor,
                    movie.Actress,
                    movie.Language,
                    movie.DurationMinutes,
                    movie.PosterUrl,
                    movie.TrailerUrl
                },
                commandType: CommandType.StoredProcedure
               );
            movie.MovieId = movieId;
            return movie;
        }
        #endregion

        #region UpdateMoviesAsync
        public async Task<bool> UpdateMoviesAsync(Movies movie)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "SP_UpdateMovie",
                new
                {
                    movie.MovieId,
                    movie.Title,
                    movie.Genre,
                    movie.ReleaseDate,
                    movie.Director,
                    movie.Description,
                    movie.Actor,
                    movie.Actress,
                    movie.Language,
                    movie.DurationMinutes,
                    movie.PosterUrl,
                    movie.TrailerUrl
                },
                commandType: CommandType.StoredProcedure
               );
            return affectedRows > 0;
        }
        #endregion

        #region DeleteMoviesAsync
        public async Task<bool> DeleteMoviesAsync(int id)
        {
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(
                "SP_DeleteMovie",
                new { MovieId = id },
                commandType: CommandType.StoredProcedure
               );
            return affectedRows > 0;
        }
        #endregion

    }
}
