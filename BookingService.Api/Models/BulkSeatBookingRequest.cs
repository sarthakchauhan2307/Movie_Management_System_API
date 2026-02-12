namespace BookingService.Api.Models
{
    public class BulkSeatBookingRequest
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public List<string> SeatNos  { get; set; } 
    }
}
