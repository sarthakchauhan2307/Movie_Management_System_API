using Microsoft.EntityFrameworkCore;
using ReviewService.Api.Models;

namespace ReviewService.Api.Data
{
    public class ReviewDbContext : DbContext
    {
        public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
        {
        }
        public DbSet<ReviewModel> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewModel>()
                .HasIndex(r => new { r.MovieId, r.UserId })
                .IsUnique(); 
        }
    }
}
