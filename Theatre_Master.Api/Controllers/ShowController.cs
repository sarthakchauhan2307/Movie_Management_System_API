using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Controllers
{
    //[Route("api/[controller]/[action]")]
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        #region configuration
        private readonly TheatreMasterDbContext _context;

        public ShowController(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetShows
        [HttpGet]
        public async Task<IActionResult> GetShows()
        {
            var shows = await _context.Shows.ToListAsync();
            return Ok(shows);
        }
        #endregion

        #region GetShowById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowById(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            return Ok(show);
        }
        #endregion

        #region AddShow
        [HttpPost]
        public async Task<IActionResult> AddShow(Show show)
        {
            show.Created = DateTime.Now;
            show.Modified = DateTime.Now;
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();
            return Ok(show);
        }
        #endregion

        #region UpdateShow
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShow(int id, Show show)
        {
            var existingShow = await _context.Shows.FindAsync(id);
            if (existingShow == null)
            {
                return NotFound();
            }
            existingShow.ShowDate = show.ShowDate;
            existingShow.ShowTime = show.ShowTime;
            existingShow.ScreenId = show.ScreenId;
            existingShow.Modified = DateTime.Now;
            _context.Shows.Update(existingShow);
            await _context.SaveChangesAsync();
            return Ok(existingShow);
        }
        #endregion

        #region DeleteShow
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion
    }
}
