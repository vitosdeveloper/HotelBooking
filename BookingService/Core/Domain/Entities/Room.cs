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
        public ICollection<Booking> Bookings { get; set; }
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
            get
            {
                var notAvailableStatuses = new List<Enums.Status>()
                {
                    Enums.Status.Created,
                    Enums.Status.Paid,
                };
                return Bookings.Where(
                    b => b.Room.Id == Id &&
                    notAvailableStatuses.Contains(b.CurrentStatus)).Count() > 0;
            }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidRoomDataException();
            }
            if (Price == null || Price.Value < 10)
            {
                throw new InvalidRoomPriceException();
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

        public bool CanBeBooked()
        {
            try
            {
                ValidateState();
            }
            catch (Exception)
            {
                return false;
            }

            if (!IsAvailable)
            {
                return true;
            }
            return false;
        }
    }
}
