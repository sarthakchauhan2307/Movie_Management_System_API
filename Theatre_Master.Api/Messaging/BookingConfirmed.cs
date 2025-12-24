namespace TheatreMasterService.Api.Messaging
{
    public class BookingConfirmed
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public int SeatCount { get; set; }

    }
}
