using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public interface IScreenRepository
    {
        Task<IEnumerable<Screen>> GetScreensAsync();
        Task<Screen?> GetScreenByIdAsync(int id);
        Task<Screen> CreateScreenAsync(Screen screen);
        Task<bool> UpdateScreenAsync(Screen screen);
        Task<bool> DeleteScreenAsync(int id);
    }
}
