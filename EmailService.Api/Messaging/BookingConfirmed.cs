namespace EmailService.Api.Messaging
{
    public class BookingConfirmed
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public int SeatCount { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShowTime { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
