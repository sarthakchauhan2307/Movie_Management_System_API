using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Service
{
    public interface ITheatreService
    {
        Task<IEnumerable<Theatre>> GetTheatresAsync();
        Task<Theatre?> GetTheatreByIdAsync(int id);
        Task<Theatre> CreateTheatreAsync(Theatre theatre);
        Task<bool> UpdateTheatreAsync(int id, Theatre theatre);
        Task<bool> DeleteTheatreAsync(int id);

    }
}
