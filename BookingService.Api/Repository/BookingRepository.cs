using BookingService.Api.Data;
using BookingService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Api.Repository
{
    public class BookingRepository : IBookingRepository
    {
        #region Configuration
        private readonly BookingServiceDbContext _context;
        public BookingRepository(BookingServiceDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetBookingAsync
        public async Task<IEnumerable<Booking>> GetBookingAsync()
        {
            return await _context.Bookings.ToListAsync();
        }
        #endregion

        #region CreateBookingAsync
        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }
        #endregion

        #region GetBookingByShowId
        public async Task<IEnumerable<Booking>> GetBookingByShowIdAsync(int showid)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ShowId == showid)
                .ToListAsync();
            return bookings;
        }
        #endregion

        #region GetBookingByIdAsync
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings.FindAsync(bookingId);
        }
        #endregion

        #region GetBookingByUserIdAsync
        public async Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
            return bookings;
        }
        #endregion

        #region UpdateBookingAsync
        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

    }
}
