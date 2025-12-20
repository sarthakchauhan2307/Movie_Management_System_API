using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public class TheatreRepository : ITheatreRepository
    {
        #region Configuration
        private readonly TheatreMasterDbContext _context;
        public TheatreRepository(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region CreateTheatreAsync
        public async Task<Theatre> CreateTheatreAsync(Theatre theatre)
        {
            theatre.Created = DateTime.Now;
            theatre.Modified = DateTime.Now;
            await _context.Theatres.AddAsync(theatre);
            await  _context.SaveChangesAsync();
            return theatre;
        }
        #endregion

        #region GetTheatresAsync
        public async Task<IEnumerable<Theatre>> GetTheatresAsync()
        {
            return await _context.Theatres.ToListAsync();
        }
        #endregion

        #region GetTheatreByIdAsync
        public async Task<Theatre?> GetTheatreByIdAsync(int id)
        {
            return await _context.Theatres.FindAsync(id);
        }
        #endregion

        #region UpdateTheatreAsync
        public async Task<bool> UpdateTheatreAsync(Theatre theatre)
        {
            var existingTheatre = await _context.Theatres.FindAsync(theatre.TheatreId);
            if (existingTheatre == null)
                return false;
            existingTheatre.TheatreName = theatre.TheatreName;
            existingTheatre.City = theatre.City;
            existingTheatre.Modified = DateTime.Now;
            _context.Theatres.Update(existingTheatre);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteTheatreAsync
        public async Task<bool> DeleteTheatreAsync(int id)
        {
            var existingTheatre = await _context.Theatres.FindAsync(id);
            if (existingTheatre == null)
                return false;
            _context.Theatres.Remove(existingTheatre);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
