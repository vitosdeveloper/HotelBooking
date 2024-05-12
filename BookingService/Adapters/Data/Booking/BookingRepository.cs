using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDBContext _dbContext;
        public BookingRepository(HotelDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Domain.Entities.Booking> Create(Domain.Entities.Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Domain.Entities.Booking> Get(int id)
        {
            return _dbContext.Bookings.Where(x => x.Id == id).FirstAsync();
        }
    }
}
