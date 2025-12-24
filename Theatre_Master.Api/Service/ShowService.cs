using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Models;
using TheatreMasterService.Api.Repository;

namespace TheatreMasterService.Api.Service
{
    public class ShowService : IShowService
    {
        #region Configuration
        private readonly IShowRepository _showRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly MicroServiceGateway _gateway;
        public ShowService(IShowRepository showRepository, MicroServiceGateway gateway , IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
            _showRepository = showRepository;
            _gateway = gateway;
        }
        #endregion

        #region GetShowsAsync
        public async Task<IEnumerable<Show>> GetShowsAsync()
        {
            return await _showRepository.GetShowsAsync();
        }
        #endregion

        #region GetShowByIdAsync
        public async Task<Show?> GetShowByIdAsync(int id)
        {
            var show = await _showRepository.GetShowByIdAsync(id);
            if (show == null)
                throw new KeyNotFoundException("Show not found");

            return show;
        }
        #endregion

        #region CreateShowAsync
        public async Task<Show> CreateShowAsync(Show show)
        {
            show.Created = DateTime.Now;
            show.Modified = DateTime.Now;
            return await _showRepository.CreateShowAsync(show);
        }
        #endregion

        #region UpdateShowAsync
        public async Task<bool> UpdateShowAsync(int id, Show show)
        {
            var existingShow = await _showRepository.GetShowByIdAsync(id);
            if (existingShow == null)
                return false;
            existingShow.ShowDate = show.ShowDate;
            existingShow.ScreenId = show.ScreenId;
            existingShow.Price = show.Price;
            existingShow.ShowTime = show.ShowTime;
            existingShow.Modified = DateTime.Now;
            return await _showRepository.UpdateShowAsync(existingShow);
        }
        #endregion

        #region DeleteShowAsync
        public async Task<bool> DeleteShowAsync(int id)
        {
            var hasBooking = await _gateway.HasBookingByShow(id);
            if (hasBooking)
                throw new InvalidOperationException("Cannot delete show with active bookings.");
            return await _showRepository.DeleteShowAsync(id);
        }
        #endregion

        #region GetShowsByMovieIdAsync
        public async Task<IEnumerable<Show>> GetShowsByMovieId(int movieId)
        {
            var allShows = await _showRepository.GetShowsByMovieId(movieId);
            return (allShows);
        }
        #endregion

        #region GetAvailableSeatsAsync
        public async Task<object> GetAvailableSeatsAsync(int showId)
        {
            // 1️ Get Show
            var show = await _showRepository.GetShowByIdAsync(showId);
            if (show == null)
                throw new Exception("Show not found");

            // 2️ Get Screen
            var screen = await _screenRepository.GetScreenByIdAsync(show.ScreenId);
            if (screen == null)
                throw new Exception("Screen not found");

            // 3️ Get booked seats from BookingService
            var bookedSeats = await _gateway.GetBookedSeatsByShowAsync(showId);

            // 4️ Calculate available seats
            var availableSeats = screen.SeatCapacity - bookedSeats;
            if (availableSeats < 0) availableSeats = 0;

            return new
            {
                ShowId = showId,
                ScreenId = screen.ScreenId,
                TotalSeats = screen.SeatCapacity,
                BookedSeats = bookedSeats,
                AvailableSeats = availableSeats
            };
        }
        #endregion

        #region ReduceSeatsAsync
        public async Task ReduceSeatsAsync(int showId, int seatCount)
        {
            var show = await _showRepository.GetShowByIdAsync(showId);
            if (show == null)
                throw new Exception("Show not found");

            Console.WriteLine(
                $"[RabbitMQ] Booking confirmed. ShowId={showId}, Seats={seatCount}");
        }
        #endregion
    }
}
