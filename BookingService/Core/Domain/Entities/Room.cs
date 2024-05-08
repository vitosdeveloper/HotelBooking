using Domain.DomainExceptions;
using System.Reflection.Metadata;
using Domain.Ports;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public bool IsAvailable
        {
            get
            {
                if (InMaintenance || !HasGuest)
                {
                    return false;
                }
                return true;
            }
        }

        public bool HasGuest
        {
            // verificar se existem bookings abertos pra essa room
            get { return true; }
        }

        private void ValidateState()
        {
            if (Name == null)
            {
                throw new InvalidRoomDataException();
            }
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            ValidateState();
            if (Id == 0)
            {
                Id = await roomRepository.Create(this);
            }
            else
            {
                //edit
            }
        }
    }
}
