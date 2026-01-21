using BookingService.Api.DTo;
using BookingService.Api.Messaging;
using BookingService.Api.Models;
using BookingService.Api.Repository;
using ClosedXML.Excel;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BookingService.Api.Services
{
    public class BookingServices: IBookingService
    {
        #region Configuration
        private readonly IDistributedCache _cache ;
        private readonly IBookingRepository _bookingRepository;
        private readonly MicroServiceGateway _gateway;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IBookingSeatRepository _seatRepository;
        public BookingServices(IDistributedCache cache ,IBookingRepository bookingRepository ,MicroServiceGateway gateway, IMessageBusClient messageBusClient, IBookingSeatRepository seatRepository)
        {
             _cache = cache;
            _bookingRepository = bookingRepository;
            _gateway = gateway;
            _messageBusClient = messageBusClient;
            _seatRepository = seatRepository;
           
        }
        #endregion

        #region GetBookingAsync
        public async Task<IEnumerable<Booking>> GetBookingAsync()
        {
            const string cacheKey = "all_bookings";

            var cachedBookings = await _cache.GetStringAsync(cacheKey);
            if (cachedBookings != null)
            {
                Console.WriteLine("Return from cache memory");
                return JsonSerializer.Deserialize<IEnumerable<Booking>>(cachedBookings)!;
            }
            var bookings = await _bookingRepository.GetBookingAsync();

            //  Store in cache (10 minutes)
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            string serializedData = JsonSerializer.Serialize(bookings);
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
            return bookings;
        }
        #endregion

        #region GetBookingByIdAsync
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
             string cacheKey = $"booking_{bookingId}";

            string? cachedBooking = await _cache.GetStringAsync(cacheKey);
            if (cachedBooking != null)
            {
                Console.WriteLine("Return from cache memory");
                return JsonSerializer.Deserialize<Booking>(cachedBooking);
            }
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);
            //  Store in cache (10 minutes)
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            string serializedData = JsonSerializer.Serialize(booking);
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

            return booking;
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

            //calculate total price
            booking.TotalAmount = booking.SeatCount * showDetails.Price;

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
            await _bookingRepository.UpdateBookingAsync(booking);
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

        #region BulkBookingAsync (TVP + REAL PRICE)
        public async Task BulkBookSeatsAsync(
      int bookingId,
      int showId,
      List<int> seatNos)
        {
            // 1️ Try locking all seats FIRST
            foreach (var seatNo in seatNos)
            {
                bool locked = await TryLockSeatsAsync(showId, seatNo);

                if (!locked)
                {
                    throw new Exception($"Seat {seatNo} is already booked or in progress");
                }
            }

            // 2️ Fetch Show price
            var showDetails = await _gateway.GetShowDetailsAsync(showId);
            int price = showDetails.Price;

            // 3️ Prepare TVP rows
            var seats = seatNos.Select(no => new BookingSeatTvpDto
            {
                SeatNo = no,
                Price = price
            }).ToList();

            // 4️ Bulk insert seats
            await _seatRepository.BulkInsertSeatsAsync(
                bookingId,
                showId,
                seats
            );
        }


        #endregion

        #region UploadExcelAndBulkInsertAsync

        public async Task UploadExcelAndBulkInsertAsync(Stream file)
        {
            var rowsData = new List<BookingSeatExcelDto>();

            using var workbook = new XLWorkbook(file);
            var sheet = workbook.Worksheet(1);

            //  Basic header check
            if (sheet.Cell(1, 1).GetString() != "BookingId" ||
                sheet.Cell(1, 2).GetString() != "ShowId" ||
                sheet.Cell(1, 3).GetString() != "SeatNo" ||
                sheet.Cell(1, 4).GetString() != "Price")
            {
                throw new Exception("Invalid Excel format");
            }

            //  Read rows
            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                if (!row.Cell(1).TryGetValue(out int bookingId) ||
                    !row.Cell(2).TryGetValue(out int showId) ||
                    !row.Cell(3).TryGetValue(out int seatNo) ||
                    !row.Cell(4).TryGetValue(out int price))
                {
                    throw new Exception($"Invalid data at row {row.RowNumber()}");
                }

                rowsData.Add(new BookingSeatExcelDto
                {
                    BookingId = bookingId,
                    ShowId = showId,
                    SeatNo = seatNo,
                    Price = price
                });
            }

            // Check duplicate seats inside Excel
            if (rowsData.GroupBy(x => new { x.ShowId, x.SeatNo }).Any(g => g.Count() > 1))
            {
                throw new Exception("Duplicate SeatNo found in Excel");
            }

            //  Group and bulk insert
            foreach (var group in rowsData.GroupBy(x => new { x.BookingId, x.ShowId }))
            {
                var seats = group.Select(x => new BookingSeatTvpDto
                {
                    SeatNo = x.SeatNo,
                    Price = x.Price
                }).ToList();

                await _seatRepository.BulkInsertSeatsAsync(
                    group.Key.BookingId,
                    group.Key.ShowId,
                    seats
                );
            }
        }

        #endregion
            

        #region GetBookedSeatsByBookingId
        public async Task<List<int>> GetBookedSeatsAsync(int bookingId)
        {
            return await _seatRepository.GetBookedSeatsByBookingIdAsync(bookingId);
        }
        #endregion


        #region LockSeatAsync
        private async Task<bool> TryLockSeatsAsync(int showId, int seatNo)
        {
            string lockKey = $"lock_show_{showId}_seat_{seatNo}";

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(100) // Lock expires in 100 seconds
            };
            // Try to set lock ONLY if it does not exist
            var existingLock = await _cache.GetStringAsync(lockKey);
            if (existingLock != null)
            {
                // Seat is already locked
                return false;
            }
            // Create lock
            await _cache.SetStringAsync(lockKey, "locked", options);
            return true;
        }
        #endregion


    }
}
