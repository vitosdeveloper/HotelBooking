using Domain.Entities;

namespace Domain.Ports
{
    public interface IBookingRepository
    {
        Task<Booking> Get(int id);
        Task<Booking> Create(Booking booking);
    }
}
