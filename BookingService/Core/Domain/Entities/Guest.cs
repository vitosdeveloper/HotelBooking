using Domain.DomainExceptions;
using Domain.Ports;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public PersonId DocumentId { get; set; }
        private void ValidateState()
        {
            if (DocumentId == null ||
                DocumentId.IdNumber.Length < 4 ||
                DocumentId.DocumentType == 0)
            {
                throw new InvalidPersonDocumentIdException();
            }

            if (Name == null || Surname == null || Email == null)
            {
                throw new MissingFieldException();
            }

            if (!Utils.ValidateEmail(Email))
            {
                throw new InvalidEmailException();
            }
        }

        public async Task Save(IGuestRepository guestRepository)
        {
            ValidateState();
            if (Id == 0)
            {
                Id = await guestRepository.Create(this);
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
