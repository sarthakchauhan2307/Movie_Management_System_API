using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieService.Api.Data;
using MovieService.Api.DTos;
using MovieService.Api.Models;
using MovieService.Api.Services;

namespace MovieService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
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
        public async Task<IActionResult> AddMovie([FromForm] MovieCreateUpdateDto dto)
        {
            
          return Ok( await _movieservice.CreateMoviesAsync(dto));
        }
        #endregion

        #region UpdateMovie
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromForm] MovieCreateUpdateDto dto)
        {
            var existingMovie = await _movieservice.GetMoviesByIdAsync(id);
            if (existingMovie == null)
                return NotFound("Movie not found");
            //dto.MovieId = id;
            bool sucess = await _movieservice.UpdateMoviesAsync(id, dto);

            return sucess
        ? NoContent()
        : NotFound("Movie not found");
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
