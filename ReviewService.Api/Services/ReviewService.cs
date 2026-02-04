using ReviewService.Api.DTos;
using ReviewService.Api.Models;
using ReviewService.Api.Repositories;

namespace ReviewService.Api.Services
{
    public class ReviewService : IReviewService
    {
        #region configuration
        private readonly IReviewRepository _repository;

            public ReviewService(IReviewRepository repository)
            {
                _repository = repository;
            }
        #endregion

        #region CreateReviewAsync
        public async Task CreateReviewAsync(CreateReviewDto dto, int userId)
            {
                if (!Enum.IsDefined(typeof(ReviewCategory), dto.Category))
                    throw new Exception("Invalid review category");

                var existingReview =
                    await _repository.GetByMovieAndUserAsync(dto.MovieId, userId);

                if (existingReview != null)
                    throw new Exception("You already reviewed this movie");

                // 3️⃣ Create entity
                var review = new ReviewModel
                {
                    MovieId = dto.MovieId,
                    UserId = userId,
                    Category = dto.Category,
                    Description = dto.Description,
                    CreatedAt = DateTime.UtcNow
                };

                // 4️⃣ Save
                await _repository.AddAsync(review);
            }
        #endregion

        #region GetReviewsByMovieAsync
        public async Task<IEnumerable<ReviewModel>> GetReviewsByMovieAsync(int movieId)
            {
                var reviews = await _repository.GetByMovieIdAsync(movieId);

                return reviews.Select(r => new ReviewModel
                {
                    ReviewId = r.ReviewId,
                    MovieId = r.MovieId,
                    UserId = r.UserId,
                    Category = r.Category,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt
                });
        }
        #endregion

        #region GetAllReviewsAsync
        public async Task<IEnumerable<ReviewModel>> GetAllReviewsAsync()
        {
            var reviews = await _repository.GetAllAsync();

            return reviews.Select(r => new ReviewModel
            {
                ReviewId = r.ReviewId,
                MovieId = r.MovieId,
                UserId = r.UserId,
                Category = r.Category,
                Description = r.Description,
                CreatedAt = r.CreatedAt
            });
        }
        #endregion

        #region UpdateReviewAsync

        public async Task UpdateReviewAsync(int reviewId, UpdateReviewDto dto, int userId)
        {
            if (!Enum.IsDefined(typeof(ReviewCategory), dto.Category))
                throw new Exception("Invalid review category");

            var review = await _repository.GetByIdAsync(reviewId);

            if (review == null)
                throw new Exception("Review not found");

            if (review.UserId != userId)
                throw new Exception("You are not allowed to update this review");

            review.Category = dto.Category;
            review.Description = dto.Description;
            review.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(review);
        }
        #endregion


    }


}
