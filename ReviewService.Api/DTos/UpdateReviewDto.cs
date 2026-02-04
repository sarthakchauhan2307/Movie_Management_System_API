using ReviewService.Api.Models;

namespace ReviewService.Api.DTos
{
    public class UpdateReviewDto
    {
        public ReviewCategory Category { get; set; }
        public string? Description { get; set; }
    }
}
