using ReviewService.Api.Models;

namespace ReviewService.Api.DTos
{
    public class CreateReviewDto
    {
        public int MovieId { get; set; }
        public ReviewCategory Category { get; set; }
        public string? Description { get; set; }
    }
}
