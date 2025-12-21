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

    }
}
    