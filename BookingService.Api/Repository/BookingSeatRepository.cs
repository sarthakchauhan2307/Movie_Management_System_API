using BookingService.Api.Data;
using BookingService.Api.DTo;
using Dapper;
using System.Data;

namespace BookingService.Api.Repository
{
    public class BookingSeatRepository : IBookingSeatRepository
    {
        #region Configuration
        private readonly DapperContext _context;

        public BookingSeatRepository(DapperContext context)
        {
            _context = context;
        }
        #endregion

        #region Bulk Insert Booking Seats (TVP)
        public async Task BulkInsertSeatsAsync(
            int bookingId,
            int showId,
            List<BookingSeatTvpDto> seats)
        {
            using var connection = _context.CreateConnection();

            // Convert List DataTable (MUST match SQL TVP)
            var table = new DataTable();
            table.Columns.Add("SeatNo", typeof(int));
            table.Columns.Add("Price", typeof(int));

            foreach (var seat in seats)
            {
                table.Rows.Add(seat.SeatNo, seat.Price);
            }

            // Prepare parameters
            var parameters = new DynamicParameters();
            parameters.Add("@BookingId", bookingId);
            parameters.Add("@ShowId", showId);
            parameters.Add(
                "@Seats",
                table.AsTableValuedParameter("BookingSeatType") 
            );

            // 3️ Execute Stored Procedure
            await connection.ExecuteAsync(
                "SP_BulkInsertBookingSeats",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
        #endregion

        #region SeeBulkResult
        public async Task<List<int>> GetBookedSeatsByBookingIdAsync(int bookingId)
        {
            using var connection = _context.CreateConnection();

            var seats = await connection.QueryAsync<int>(
                "SELECT SeatNo FROM BookingSeats WHERE BookingId = @BookingId",
                new { BookingId = bookingId });

            return seats.ToList();
        }
        #endregion

    }
}
