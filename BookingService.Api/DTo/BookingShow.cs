namespace BookingService.Api.DTo
{
    public class BookingShow
    {
        public int ShowId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShowTime { get; set; }
        public int MovieId { get; set; }
        public int Price { get; set; }
    }
}
