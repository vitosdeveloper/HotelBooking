using Application;
using Application.Guest.DTO;
using Application.Guest.Requests;
using Domain.Entities;
using Domain.Ports;

namespace ApplicationTests
{
    class FakeRepo : IGuestRepository
    {
        public Task<int> Create(Guest guest)
        {
            return Task.FromResult(111);
        }

        public Task<Guest> Get(int Id)
        {
            throw new NotImplementedException();
        }
    }

    public class Tests
    {
        GuestManager guestManager;

        [SetUp]
        public void Setup()
        {
            guestManager = new(new FakeRepo());
        }

        [Test]
        public async Task HappyPath()
        {
            var guestDto = new GuestDTO
            {
                Name = "Vitor",
                Surname = "Fernandes",
                Email = "vitosdeveloper@gmail.com",
                IdNumber = "abcd",
                IdTypeCode = 1
            };
            int expectedId = 111;

            var request = new CreateGuestRequest() { Data = guestDto };

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.IsTrue(res.Success);
            Assert.That(res.Data.Id, Is.EqualTo(expectedId));
            Assert.That(res.Data.Name, Is.EqualTo(guestDto.Name));
        }

        [TestCase("")]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task ShouldReturnInvalidPersonDocumentIdExceptionWhenDocsAreInvalid(string docNumber)
        {
            var guestDto = new GuestDTO
            {
                Name = "Vitor",
                Surname = "Fernandes",
                Email = "vitosdeveloper@gmail.com",
                IdNumber = docNumber,
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest() { Data = guestDto };

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.INVALID_PERSON_ID));
            Assert.That(res.Message, Is.EqualTo("The given ID isnt valid."));
        }

        [TestCase("", "surnametest", "email@email.com")]
        [TestCase(null, "surnametest", "email@email.com")]
        [TestCase("nametest", "", "email@email.com")]
        [TestCase("nametest", null, "email@email.com")]
        [TestCase("nametest", "surnametest", "")]
        [TestCase("nametest", "surnametest", null)]
        public async Task ShouldReturnMisingRequiredInformation(string name, string surname, string email)
        {
            var guestDto = new GuestDTO
            {
                Name = name,
                Surname = surname,
                Email = email,
                IdNumber = "abcd",
                IdTypeCode = 1
            };

            var request = new CreateGuestRequest() { Data = guestDto };

            var res = await guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.MISSING_REQUIRED_INFORMATION));
            Assert.That(res.Message, Is.EqualTo("Missing required information."));
        }
    }
}