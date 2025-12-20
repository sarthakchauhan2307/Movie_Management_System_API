        using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Service
{
    public interface IScreenService
    {
        Task<IEnumerable<Screen>> GetScreensAsync();
        Task<Screen?> GetScreenByIdAsync(int id);
        Task<Screen> CreateScreenAsync(Screen screen);
        Task<bool> UpdateScreenAsync(int id, Screen screen);
        Task<bool> DeleteScreenAsync(int id);

    }
}
