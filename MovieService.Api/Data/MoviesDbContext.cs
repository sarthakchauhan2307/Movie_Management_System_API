using Microsoft.EntityFrameworkCore;
using MovieService.Api.Models;

namespace MovieService.Api.Data
{
    public class MoviesDbContext : DbContext
    {
         public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options) { }
    
          public DbSet<Movies> Movies { get; set; }
    
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
                modelBuilder.Entity<Movies>()
                 .HasIndex(m => m.Title);
                base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movies>(e =>
            {
                e.HasKey(m => m.MovieId);
                e.HasIndex(m => m.Title);
            });

          }

    }
}
