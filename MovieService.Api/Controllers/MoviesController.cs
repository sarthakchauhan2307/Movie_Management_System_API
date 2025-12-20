using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Data;
using MovieService.Api.Models;
using MovieService.Api.Services;

namespace MovieService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        #region configuration
        private readonly IMoviesService _movieservice;

        public MoviesController(IMoviesService movieservice)
        {
            _movieservice = movieservice;
        }
        #endregion

        #region GetMovies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieservice.GetMoviesAsync();
            return Ok(movies);
        }
        #endregion

        #region AddMovie
        [HttpPost]
        public async Task<IActionResult> AddMovie(Movies movie)
        {
            
          return Ok( await _movieservice.CreateMoviesAsync(movie));
        }
        #endregion

        #region UpdateMovie
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movies movie)
        {
            var result = await _movieservice.UpdateMoviesAsync(id, movie);
            if (!result)
                return NotFound("Movie not found");

            return Ok("Movie updated successfully");
        }
        #endregion

        #region DeleteMovie
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            try
            {
                await _movieservice.DeleteMoviesAsync(id);
                return Ok("Movie deleted successfully");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetMovieById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
           var movie = await _movieservice.GetMoviesByIdAsync(id);
            if (movie == null)
                return NotFound("Movie not found");
            return Ok(movie);
        }
        #endregion

    }
}
