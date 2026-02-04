using ReviewService.Api.DTos;
using ReviewService.Api.Models;

namespace ReviewService.Api.Services
{
    public interface IReviewService
    {
        Task CreateReviewAsync(CreateReviewDto dto, int userId);
        Task<IEnumerable<ReviewModel>> GetReviewsByMovieAsync(int movieId);
        Task<IEnumerable<ReviewModel>> GetAllReviewsAsync();

        Task UpdateReviewAsync(int reviewId, UpdateReviewDto dto, int userId);

    }
}
