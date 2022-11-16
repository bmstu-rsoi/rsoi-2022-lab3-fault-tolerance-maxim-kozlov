using Microsoft.EntityFrameworkCore;
using FlightBooking.FlightService.Database.Entities;

namespace FlightBooking.FlightService.Database
{
    public class FlightsContext : DbContext
    {
        public FlightsContext()
        {
        }

        public FlightsContext(DbContextOptions<FlightsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Airport> Airports { get; set; } = null!;
        public virtual DbSet<Flight> Flights { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasOne(d => d.FromAirport)
                    .WithMany(p => p.FlightFromAirports)
                    .HasForeignKey(d => d.FromAirportId)
                    .HasConstraintName("flight_from_airport_id_fkey");

                entity.HasOne(d => d.ToAirport)
                    .WithMany(p => p.FlightToAirports)
                    .HasForeignKey(d => d.ToAirportId)
                    .HasConstraintName("flight_to_airport_id_fkey");
            });
        }
    }
}
