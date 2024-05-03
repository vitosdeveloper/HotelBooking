namespace Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }

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
    }
}
