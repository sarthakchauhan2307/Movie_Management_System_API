using TheatreMaster.Api.Models;
using TheatreMasterService.Api.Repository;

namespace TheatreMasterService.Api.Service
{
    public class TheatreService : ITheatreService
    {
        #region Configuration
        private readonly ITheatreRepository _theatreRepository;
        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }
        #endregion

        #region CreateTheatreAsync
        public async Task<Theatre> CreateTheatreAsync(Theatre theatre)
        {
            return await _theatreRepository.CreateTheatreAsync(theatre);
        }
        #endregion

        #region DeleteTheatreAsync
        public async Task<bool> DeleteTheatreAsync(int id)
        {
            return await _theatreRepository.DeleteTheatreAsync(id);
        }
        #endregion

        #region GetTheatreByIdAsync
        public async Task<Theatre?> GetTheatreByIdAsync(int id)
        {
            return await _theatreRepository.GetTheatreByIdAsync(id);
        }
        #endregion

        #region GetTheatresAsync
        public async Task<IEnumerable<Theatre>> GetTheatresAsync()
        {
            return await _theatreRepository.GetTheatresAsync();
        }
        #endregion

        #region UpdateTheatreAsync
        public async Task<bool> UpdateTheatreAsync(int id, Theatre theatre)
        {
            var existingTheatre = await _theatreRepository.GetTheatreByIdAsync(id);
            if (existingTheatre == null)
                return false;
            existingTheatre.TheatreName = theatre.TheatreName;
            existingTheatre.City = theatre.City;
            return await _theatreRepository.UpdateTheatreAsync(existingTheatre);
        }
        #endregion

    }
}
