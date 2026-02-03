using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;
using TheatreMasterService.Api.Service;

namespace TheatreMasterService.Api.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("/api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class ShowController : ControllerBase
    {
        #region configuration
        private readonly IShowService _service;

        public ShowController(IShowService service)
        {
            _service = service;
        }
        #endregion

        #region GetShows
        [HttpGet]
        public async Task<IActionResult> GetShows()
            => Ok(await _service.GetShowsAsync());
        #endregion

        #region GetShowById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
             => Ok(await _service.GetShowByIdAsync(id));
        #endregion

        #region AddShow
        [HttpPost]
        public async Task<IActionResult> AddShow(Show show)
          => Ok(await _service.CreateShowAsync(show));
        #endregion

        #region UpdateShow
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(int id, Show show)
        => Ok(await _service.UpdateShowAsync(id, show));
        #endregion

        #region DeleteShow
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
             => Ok(await _service.DeleteShowAsync(id));
        #endregion

        #region GetShowsByMovieId
        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetShowsByMovieId(int movieId)
                => Ok(await _service.GetShowsByMovieId(movieId));

        #endregion

        #region GetAvailableSeats
        [HttpGet("{showId}/available-seats")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            return Ok(await _service.GetAvailableSeatsAsync(showId));
        }
        #endregion
    }
}
