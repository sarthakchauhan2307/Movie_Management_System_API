using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetShowsAsync();
        Task<Show?> GetShowByIdAsync(int id);
        Task<Show> CreateShowAsync(Show show);
        Task<bool> UpdateShowAsync(Show show);
        Task<bool> DeleteShowAsync(int id);
        Task<IEnumerable<Show>> GetShowsByMovieId(int movieId);
    }
}
