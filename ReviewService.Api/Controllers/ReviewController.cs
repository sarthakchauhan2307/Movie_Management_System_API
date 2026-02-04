using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Api.DTos;
using ReviewService.Api.Services;

namespace ReviewService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        #region Configuration
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        #endregion

        #region AddReview
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto dto,int userId)
        {
            //int userId = int.Parse(User.FindFirst("userId")!.Value);

            await _reviewService.CreateReviewAsync(dto ,userId);
            return Ok("Review added successfully");
        }
        #endregion

        #region GetReviewByMovieId

        [HttpGet("movie/{movieId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(int movieId)
        {
            var reviews = await _reviewService.GetReviewsByMovieAsync(movieId);
            return Ok(reviews);
        }
        #endregion

        #region Update
        [HttpPut("{reviewId}")]
        //[Authorize]
        public async Task<IActionResult> UpdateReview(int reviewId,UpdateReviewDto dto,int userId)
        {
            //int userId = int.Parse(User.FindFirst("userId")!.Value);

            await _reviewService.UpdateReviewAsync(reviewId, dto, userId);
            return Ok("Review updated successfully");
        }
        #endregion

        #region GetAllReviews 
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }
        #endregion
    }
}
