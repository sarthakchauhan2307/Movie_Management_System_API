using BookingService.Api.DTo;

namespace BookingService.Api.Repository
{
    public interface IBookingSeatRepository
    {
        Task BulkInsertSeatsAsync(
            int BookingId,
            int ShowId,
            List<BookingSeatTvpDto> seats);
        Task<List<string>> GetBookedSeatsByBookingIdAsync(int bookingId);

    }
}
