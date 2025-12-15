using Microsoft.EntityFrameworkCore;
using UserService.Api.Model;

namespace UserService.Api.Data
{
    public class UserDbContext : DbContext
    {
       public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            base.OnModelCreating(modelBuilder);

        }


    }
}
