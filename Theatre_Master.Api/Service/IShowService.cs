using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Service
{
    public interface IShowService
    {
        Task<IEnumerable<Show>> GetShowsAsync();
        Task<Show?> GetShowByIdAsync(int id);
        Task<Show> CreateShowAsync(Show show);
        Task<bool> UpdateShowAsync(int id, Show show);
        Task<bool> DeleteShowAsync(int id);
        Task<IEnumerable<Show>> GetShowsByMovieId(int movieId);
    }
}
