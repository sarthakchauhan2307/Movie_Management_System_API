using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Models;

namespace TheatreMaster.Api.Data
{
    public class TheatreMasterDbContext : DbContext
    {
        public TheatreMasterDbContext(DbContextOptions<TheatreMasterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Show> Shows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // =========================
            // THEATRE
            // =========================
            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.HasKey(t => t.TheatreId);

                entity.HasIndex(t => t.TheatreName)
                      .IsUnique();

                entity.HasMany(t => t.Screens)
                      .WithOne(s => s.Theatre)
                      .HasForeignKey(s => s.TheatreId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // SCREEN
            // =========================
            modelBuilder.Entity<Screen>(entity =>
            {
                entity.HasKey(s => s.ScreenId);

                // One Theatre → Many Screens
                entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                      .IsUnique();

                entity.HasMany(s => s.Shows)
                      .WithOne(sh => sh.Screen)
                      .HasForeignKey(sh => sh.ScreenId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // SHOW
            // =========================
            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.ShowId);

                // Prevent duplicate shows
                entity.HasIndex(s => new
                {
                    s.MovieId,
                    s.ScreenId,
                    s.ShowDate,
                    s.ShowTime
                }).IsUnique();

                //// Money precision
                //entity.Property(s => s.Price)
                //      .HasPrecision(10, 2);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
