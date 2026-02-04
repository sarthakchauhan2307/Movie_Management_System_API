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

        public async Task<IEnumerable<ReviewModel>> GetAllAsync()
        {
            return await _context.Reviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ReviewModel?> GetByMovieAndUserAsync(int movieId, int userId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
        }

        public async Task<IEnumerable<ReviewModel>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(ReviewModel review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReviewModel review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task<ReviewModel?> GetByIdAsync(int reviewId)
        {
            return await _context.Reviews.FindAsync(reviewId);
        }

    }
}
