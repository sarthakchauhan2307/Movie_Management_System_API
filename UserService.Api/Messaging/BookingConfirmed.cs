namespace UserService.Api.Messaging
{
    public class BookingConfirmed
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public int SeatCount { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ShowTime { get; set; }
    }
}
    