namespace BookingService.Api.DTo
{
    public class ScreenDto
    {
        public int ScreenId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public int TheatreId { get; set; }
    }
}
