using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewService.Api.Models
{
    public class ReviewModel
    {
        [Key]
        public int ReviewId { get; set; }   // Primary Key (INT)

        [Required]
        public int MovieId { get; set; }  // From Movie Service

        [Required]
        public int UserId { get; set; }   // From Auth Service

        [Required]
        public ReviewCategory Category { get; set; } // Enum → INT

        [MaxLength(1000)]
        public string? Description { get; set; } // Optional

        public DateTime CreatedAt { get; set; } 

        public DateTime? UpdatedAt { get; set; }
    }
}
