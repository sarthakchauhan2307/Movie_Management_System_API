using BookingService.Api.Models;

namespace BookingService.Api.Repository
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingAsync();
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId);
        Task<IEnumerable<Booking>> GetBookingByShowIdAsync(int showId);
        Task<bool> UpdateBookingAsync(Booking booking);

        Task<int> GetBookedSeatCountByShowAsync(int showId);
    }
}
