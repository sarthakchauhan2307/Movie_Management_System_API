using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Data;
using ReviewService.Api.Models;

namespace ReviewService.Api.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        #region Configuration
        private readonly ReviewDbContext _context;
        public ReviewRepository(ReviewDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllAsync
        public async Task<IEnumerable<ReviewModel>> GetAllAsync()
        {
            return await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        #endregion

        #region GetByMovieAndUserAsync

        public async Task<ReviewModel?> GetByMovieAndUserAsync(int movieId, int userId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
        }
        #endregion

        #region GetByMovieIdAsync

        public async Task<IEnumerable<ReviewModel>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
        #endregion

        #region AddAsync

        public async Task AddAsync(ReviewModel review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region UpdateAsync

        public async Task UpdateAsync(ReviewModel review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region GetByIdAsync

        public async Task<ReviewModel?> GetByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }
        #endregion

    }
}
