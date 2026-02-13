namespace BookingService.Api.Models
{
    public class TicketPdfModel
    {
        public int BookingId { get; set; }
        public string MovieName { get; set; }
        public string TheatreName { get; set; }
        public string Screen { get; set; }
        //public string Seats { get; set; }
        public int SeatCount { get; set; }
        public string ShowTime { get; set; }
        public decimal Amount { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public DateTime ShowDate { get;set; }
        public byte[]? MoviePoster { get; set; } = null;
        public byte[]? QrCodeImage { get; set; } = null;
        public List<string>? SeatNos { get; set; } = new();

    }
}
