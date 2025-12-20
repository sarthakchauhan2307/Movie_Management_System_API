using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public class ScreenRepository : IScreenRepository
    {
        #region Configuration
        private readonly TheatreMasterDbContext _context;
        public ScreenRepository(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region CreateScreenAsync
        public async Task<Screen> CreateScreenAsync(Screen screen)
        {
            _context.Screens.Add(screen);
            await  _context.SaveChangesAsync();
            return screen;
        }
        #endregion

        #region DeleteScreenAsync
        public async Task<bool> DeleteScreenAsync(int id)
        {
            var screen = await _context.Screens.FindAsync(id);
            if (screen == null)
                return false;
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region GetScreenByIdAsync
        public async Task<Screen?> GetScreenByIdAsync(int id)
        {
            return await _context.Screens.FindAsync(id);
        }
        #endregion

        #region GetScreensAsync
        public async Task<IEnumerable<Screen>> GetScreensAsync()
        {
            return await _context.Screens.ToListAsync();
        }
        #endregion

        #region UpdateScreenAsync
        public async Task<bool> UpdateScreenAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
