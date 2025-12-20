using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public interface ITheatreRepository
    {
        Task<IEnumerable<Theatre>> GetTheatresAsync();
        Task<Theatre?> GetTheatreByIdAsync(int id);
        Task<Theatre> CreateTheatreAsync(Theatre theatre);
        Task<bool> UpdateTheatreAsync(Theatre theatre);
        Task<bool> DeleteTheatreAsync(int id);
    }
}
