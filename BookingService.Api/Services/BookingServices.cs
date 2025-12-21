using BookingService.Api.Models;
using BookingService.Api.Repository;

namespace BookingService.Api.Services
{
    public class BookingServices : IBookingService
    {
        #region Configuration
        private readonly IBookingRepository _bookingRepository;
        private readonly MicroServiceGateway _gateway;
        public BookingServices(IBookingRepository bookingRepository ,MicroServiceGateway gateway)
        {
            _bookingRepository = bookingRepository;
            _gateway = gateway;
        }
        #endregion

        #region GetBookingAsync
        public async Task<IEnumerable<Booking>> GetBookingAsync()
        {
            return await _bookingRepository.GetBookingAsync();
        }
        #endregion

        #region GetBookingByIdAsync
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingByIdAsync(bookingId);
        }
        #endregion

        #region CreateBookingAsync
        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            booking.PaymentStatus = "Completed";
            booking.Created = DateTime.Now;
            booking.Modified = DateTime.Now;
            return await _bookingRepository.CreateBookingAsync(booking);
        }
        #endregion

        #region GetBookingByUserIdAsync
        public async Task<IEnumerable<Booking>> GetBookingByUserIdAsync(int userId)
        {
            return await _bookingRepository.GetBookingByUserIdAsync(userId);
        }
        #endregion

        #region GetBookingByShowIdAsync
        public async Task<IEnumerable<Booking>> GetBookingByShowIdAsync(int showId)
        {
            return await _bookingRepository.GetBookingByShowIdAsync(showId);
        }
        #endregion

        #region CancelBookingAsync
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return false;
            }
            booking.PaymentStatus = "Cancelled";
            booking.Modified = DateTime.Now;
            await _bookingRepository.CreateBookingAsync(booking);
            return true;
        }
        #endregion

        #region Update Booking Status
        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return false;

            booking.PaymentStatus = status;
            booking.Modified = DateTime.Now;

            await _bookingRepository.UpdateBookingAsync(booking);
            return true;
        }
        #endregion


    }
}
