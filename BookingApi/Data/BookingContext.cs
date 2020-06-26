using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookingApi.Data
{
    public class BookingContext : IdentityDbContext<IdentityUser>
    {
        public BookingContext(DbContextOptions<BookingContext> opt) : base(opt)
        {

        }

        public DbSet<Airport> Airports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Airport>().ToTable("Airport");
        }
    }
}
