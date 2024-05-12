using Domain.Entities;

namespace Domain.Ports
{
    public interface IRoomRepository
    {
        Task<Room> Get(int Id);
        Task<Room> GetAggregate(int Id);
        Task<int> Create(Room room);
    }
}
