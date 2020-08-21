using BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BookingApi.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> opt) : base(opt)
        {

        }

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Departure> Departures { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Airport>().ToTable("Airport");
            modelBuilder.Entity<Departure>().ToTable("Departure");
            modelBuilder.Entity<Destination>().ToTable("Destination");
            modelBuilder.Entity<Flight>().ToTable("Flight");
            modelBuilder.Entity<Seat>().ToTable("Seat");
            modelBuilder.Entity<Booking>().ToTable("Booking");
            modelBuilder.Entity<Passenger>().ToTable("Passenger");
            
            modelBuilder.Entity<Seat>()
                .HasIndex(s => new {s.SeatNum, FlightID = s.FlightId})
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new {b.PassengerId})
                .IsUnique();
        }
    }
}
