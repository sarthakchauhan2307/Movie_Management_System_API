using BookingService.Api.Messaging;
using BookingService.Api.Models;
using BookingService.Api.Repository;

namespace BookingService.Api.Services
{
    public class BookingServices : IBookingService
    {
        #region Configuration
        private readonly IBookingRepository _bookingRepository;
        private readonly MicroServiceGateway _gateway;
        private readonly IMessageBusClient _messageBusClient;
        public BookingServices(IBookingRepository bookingRepository ,MicroServiceGateway gateway, IMessageBusClient messageBusClient)
        {
            _bookingRepository = bookingRepository;
            _gateway = gateway;
            _messageBusClient = messageBusClient;
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

            // 1️ Save booking
            var createdBooking = await _bookingRepository.CreateBookingAsync(booking);

            // 2️ Get Show + Movie details
            var showDetails =
                await _gateway.GetShowDetailsAsync(createdBooking.ShowId);

            //adding movie
            var movie = await _gateway.GetMovieAsync(showDetails.MovieId);

            // 3️ Publish RabbitMQ event
            var eventMessage = new BookingConfirmed
            {
                BookingId = createdBooking.BookingId,
                ShowId = createdBooking.ShowId,
                SeatCount = createdBooking.SeatCount,
                Title = movie.Title,
                ShowTime = showDetails.ShowTime

            };

            await _messageBusClient.PublishConfirmedBookingAsync(eventMessage);

            return createdBooking;
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

        #region GetBookedSeatCountByShowAsync
        public async Task<int> GetBookedSeatCountByShowAsync(int showId)
        {
            // Business rule layer (can extend later)
            return await _bookingRepository.GetBookedSeatCountByShowAsync(showId);
        }
        #endregion


    }
}
