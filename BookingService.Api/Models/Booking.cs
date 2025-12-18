using System.ComponentModel.DataAnnotations;

namespace BookingService.Api.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }
        [Required]
        public int UserId { get; set; }  
        [Required]
        public int ShowId { get; set; }

        [Required]
        public int SeatCount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public int TotalAmount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }
}
