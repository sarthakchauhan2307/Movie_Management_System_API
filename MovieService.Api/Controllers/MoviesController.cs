using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Data;
using MovieService.Api.Models;

namespace MovieService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        #region configuration
        private readonly MoviesDbContext _context;

        public MoviesController(MoviesDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetMovies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }
        #endregion

        #region AddMovie
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movies movie)
        {
            
            movie.Created = DateTime.Now;
            movie.Modified = DateTime.Now;
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return Ok(movie);
        }
        #endregion

        #region UpdateMovie
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movies movie)
        {
            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }
            existingMovie.Title = movie.Title;
            existingMovie.Genre = movie.Genre;
            existingMovie.DurationMinutes = movie.DurationMinutes;
            existingMovie.Modified = DateTime.Now;
            _context.Movies.Update(existingMovie);
            await _context.SaveChangesAsync();
            return Ok(existingMovie);
        }
        #endregion

        #region DeleteMovie
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var existingMovie = await _context.Movies.FindAsync(id);
            if (existingMovie == null)
            {
                return NotFound("Movie not found.");
            }
            _context.Movies.Remove(existingMovie);
            await _context.SaveChangesAsync();
            return Ok("Movie deleted successfully.");
        }
        #endregion

        #region GetMovieById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
        #endregion

    }
}
