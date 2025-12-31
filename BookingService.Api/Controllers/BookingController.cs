using BookingService.Api.Data;
using BookingService.Api.Models;
using BookingService.Api.Services;
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
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }
        #endregion

        #region GetBookings
        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _service.GetBookingAsync();
            return Ok(bookings);
        }
        #endregion

        #region GetBookingById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _service.GetBookingByIdAsync(id);
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
            var bookings = await _service.GetBookingByUserIdAsync(userId);
            return Ok(bookings);
        }
        #endregion

        #region CancelBooking
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _service.CancelBookingAsync(id);
            return Ok(booking);
        }
        #endregion

        #region CreateBooking
        [HttpPost]
        public async Task<IActionResult> CreateBooking(Booking booking)
        {
            var createdBooking = await _service.CreateBookingAsync(booking);
            return Ok(booking);
        }
        #endregion

        #region GetBookingsByShowId
        [HttpGet("show/{showId}")]
        public async Task<IActionResult> GetBookingsByShow(int showId)
        {
            var bookings = await _service.GetBookingByShowIdAsync(showId);

            return Ok(bookings);
        }
        #endregion

        #region UpdateBookingStatus
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateBookingStatus(int id, string PaymentStatus)
        {
           var result = await _service.UpdateBookingStatusAsync(id, PaymentStatus);
              return Ok(result);
        }
        #endregion

        #region GetBookedSeatCountByShow
        [HttpGet("show/{showId}/seatcount")]
        public async Task<IActionResult> GetBookedSeatCount(int showId)
        {
            var count = await _service.GetBookedSeatCountByShowAsync(showId);
            return Ok(count);
        }
        #endregion

        #region BulkSeatBooking
        [HttpPost]
        public async Task<IActionResult> BulkBookSeats(
            BulkSeatBookingRequest request)
        {
            await _service.BulkBookSeatsAsync(
                request.BookingId,
                request.ShowId,
                request.SeatNos
            );

            return Ok("Seats booked successfully using TVP");
        }
        #endregion

        #region UploadExcel
        [HttpPost("upload-excel")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            using (var stream = file.OpenReadStream())
            {
                await _service.UploadExcelAndBulkInsertAsync(stream);
            }
            return Ok("Seats booked successfully from Excel file.");
        }
        #endregion

        #region GetBookedSeatsByBookingId
        [HttpGet("{bookingId}/seats")]
        public async Task<IActionResult> GetBookedSeats(int bookingId)
        {
            var seats = await _service.GetBookedSeatsAsync(bookingId);
            return Ok(seats);
        }
        #endregion


    }
}
