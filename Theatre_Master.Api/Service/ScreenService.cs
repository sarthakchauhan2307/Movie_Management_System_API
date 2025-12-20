using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;
using TheatreMasterService.Api.Repository;

namespace TheatreMasterService.Api.Service
{
    public class ScreenService : IScreenService
    {
        #region Configuration
        private readonly IScreenRepository _screenRepository;
        private readonly TheatreMasterDbContext _context;

        public ScreenService(IScreenRepository screenRepository, TheatreMasterDbContext context)
        {
            _screenRepository = screenRepository;
            _context = context;
        }
        #endregion

        #region GetScreensAsync
        public async Task<IEnumerable<Screen>> GetScreensAsync()
        {
            return await _screenRepository.GetScreensAsync();
        }
        #endregion

        #region GetScreenByIdAsync
        public async Task<Screen?> GetScreenByIdAsync(int id)
        {
            return await _screenRepository.GetScreenByIdAsync(id);
        }
        #endregion

        #region CreateScreenAsync
        public async Task<Screen> CreateScreenAsync(Screen screen)
        {
            screen.Created = DateTime.Now;
            screen.Modified = DateTime.Now;
            return await _screenRepository.CreateScreenAsync(screen);
        }
        #endregion

        #region UpdateScreenAsync
        public async Task<bool> UpdateScreenAsync(int id, Screen screen)
        {
            var existingScreen = await _screenRepository.GetScreenByIdAsync(id);
            if (existingScreen == null)
                throw new KeyNotFoundException("Screen not found");

            existingScreen.SeatCapacity = screen.SeatCapacity;
            existingScreen.ScreenName = screen.ScreenName;
            existingScreen.TheatreId = screen.TheatreId;
            existingScreen.Modified = DateTime.Now;
            return await _screenRepository.UpdateScreenAsync(existingScreen);
        }
        #endregion

        #region DeleteScreenAsync
        public async Task<bool> DeleteScreenAsync(int id)
        {
            var hasShows = await _context.Shows.AnyAsync(s => s.ScreenId == id);
            if (hasShows)
                throw new InvalidOperationException("Cannot delete screen with shows");

            return await _screenRepository.DeleteScreenAsync(id);
        }
        #endregion
    }
}
