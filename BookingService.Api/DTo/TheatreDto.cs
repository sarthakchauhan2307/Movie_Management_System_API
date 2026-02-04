namespace BookingService.Api.DTo
{
    public class TheatreDto
    {
        public int TheatreId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
