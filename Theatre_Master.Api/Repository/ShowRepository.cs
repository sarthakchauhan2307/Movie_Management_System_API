using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;

namespace TheatreMasterService.Api.Repository
{
    public class ShowRepository : IShowRepository
    {
        #region Configuration
        private readonly TheatreMasterDbContext _context;
        public ShowRepository(TheatreMasterDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetShowsAsync
        public async Task<IEnumerable<Show>> GetShowsAsync()
        {
            return await _context.Shows.ToListAsync();
        }
        #endregion

        #region GetShowByIdAsync
        public async Task<Show?> GetShowByIdAsync(int id)
        {
            return await _context.Shows.FindAsync(id);
        }
        #endregion

        #region AddShowAsync
        public async Task<Show> CreateShowAsync(Show show)
        {
            await _context.Shows.AddAsync(show);
            await _context.SaveChangesAsync();
            return show;
        }
        #endregion

        #region UpdateShowAsync
        public async Task<bool> UpdateShowAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region DeleteShowAsync
        public async Task<bool> DeleteShowAsync(int id)
        {
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
                return false;
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

        #region GetShowsByMovieId
        public async Task<IEnumerable<Show>> GetShowsByMovieId(int movieId)
        {
            return await _context.Shows
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }
        #endregion
    }
}
