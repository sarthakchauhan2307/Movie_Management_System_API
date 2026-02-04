using ReviewService.Api.Models;

namespace ReviewService.Api.Repositories
{
    public interface IReviewRepository
    {
        Task<ReviewModel?> GetByMovieAndUserAsync(int movieId, int userId);
        Task<IEnumerable<ReviewModel>> GetByMovieIdAsync(int movieId);
        Task AddAsync(ReviewModel review);
        Task UpdateAsync(ReviewModel review);
        Task<IEnumerable<ReviewModel>> GetAllAsync();
        Task<ReviewModel?> GetByIdAsync(int reviewId);

    }
}
