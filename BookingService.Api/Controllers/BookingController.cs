using BookingService.Api.Data;
using BookingService.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        #region configuration
        private readonly BookingServiceDbContext _context;

        public BookingController(BookingServiceDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetBookings
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _context.Bookings.ToListAsync();
            return Ok(bookings);
        }
        #endregion

        #region GetBookingById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }
        #endregion

        #region GetBookingsByUserId
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(int userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .ToListAsync();
            return Ok(bookings);
        }
        #endregion

        #region CancelBooking
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.PaymentStatus = "Cancelled";
            booking.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(booking);
        }
        #endregion

        #region CreateBooking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            booking.Created = DateTime.Now;
            booking.Modified = DateTime.Now;
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return Ok(booking);
        }
        #endregion

        #region GetBookingsByShowId
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShow(int showId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ShowId == showId && b.PaymentStatus == "Completed")
                .ToListAsync();

            return Ok(bookings);
        }
        #endregion

        #region UpdateBookingStatus
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, string PaymentStatus)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.PaymentStatus = PaymentStatus;
            booking.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(booking);
        }
        #endregion

       
    }
}
