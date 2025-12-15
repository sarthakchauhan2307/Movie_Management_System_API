using BookingService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Api.Data
{
    public class BookingServiceDbContext : DbContext
    {
        public BookingServiceDbContext(DbContextOptions<BookingServiceDbContext> options) : base(options)
        {
        }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                // Primary Key
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.UserId).IsRequired();
                entity.Property(b => b.ShowId).IsRequired();
            });
        }
    }    
}       
