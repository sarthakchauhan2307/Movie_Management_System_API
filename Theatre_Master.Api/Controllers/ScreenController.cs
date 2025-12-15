using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Controllers
{
    [Route("/api/[controller]/[action]")]

    [ApiController]
    public class ScreenController : ControllerBase
    {
        #region configuration
        private readonly TheatreMasterDbContext _context;

        public ScreenController(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetScreens
        [HttpGet]
        public async Task<IActionResult> GetScreens()
        {
            var screens = await _context.Screens.ToListAsync();
            return Ok(screens);
        }
        #endregion

        #region GetScreenById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null)
            {
                return NotFound();
            }
            return Ok(screen);
        }
        #endregion

        #region AddScreen
        [HttpPost]
        public async Task<IActionResult> AddScreen(Screen screen)
        {
            screen.Created = DateTime.Now;
            screen.Modified = DateTime.Now;
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();
            return Ok(screen);
        }
        #endregion

        #region UpdateScreen
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(int id, Screen screen)
        {
            var existingScreen = await _context.Screens.FindAsync(id);
            if (existingScreen == null)
            {
                return NotFound();
            }
            existingScreen.ScreenName = screen.ScreenName;
            existingScreen.ScreenType = screen.ScreenType;
            existingScreen.Modified = DateTime.Now;
            _context.Screens.Update(existingScreen);
            await _context.SaveChangesAsync();
            return Ok(existingScreen);
        }
        #endregion

        #region DeleteScreen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null)
            {
                return NotFound();
            }
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion



    }
}
