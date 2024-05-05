using Data.Guest;
using Data.Room;
using ;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class HotelDBContext : DbContext
    {
        public HotelDBContext(DbContextOptions<HotelDBContext> options) : base(options) { }

        public virtual DbSet<Domain.Entities.Guest> Guests { get; set; }
        public virtual DbSet<Domain.Entities.Room> Rooms { get; set; }
        public virtual DbSet<Domain.Entities.Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GuestConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}
