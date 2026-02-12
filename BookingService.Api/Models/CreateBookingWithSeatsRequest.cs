namespace BookingService.Api.Models
{
    public class CreateBookingWithSeatsRequest
    {
        public int ShowId { get; set; }
        public int UserId { get; set; }
        public List<string> SeatNos { get; set; } = new();
    }
}
