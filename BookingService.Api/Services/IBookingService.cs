using BookingService.Api.Models;

namespace BookingService.Api.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingAsync();

        Task<Booking?> GetBookingByIdAsync(int bookingId);

        Task<Booking> CreateBookingAsync(Booking booking);

        Task<bool> CancelBookingAsync(int bookingId);

        Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId);

        Task<IEnumerable<Booking>> GetBookingByShowIdAsync(int showId);

        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);

        Task<int> GetBookedSeatCountByShowAsync(int showId);

        Task BulkBookSeatsAsync(
            int bookingId,
            int showId,
            List<string> seatNos
        );

        Task UploadExcelAndBulkInsertAsync(Stream file);

        Task<List<string>> GetBookedSeatsAsync(int bookingId);

        Task<TicketPdfModel> BuildTicketPdfModelAsync(int bookingId);

        byte[] GenerateTicketPdf(TicketPdfModel ticket);
        Task<int> CreateBookingWithSeatsAsync(CreateBookingWithSeatsRequest request);

    }
}
