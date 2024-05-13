
using Application.Booking;
using Application.Booking.DTO;
using Application.Payment.Dtos;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.Ports;
using Moq;

namespace ApplicationTests
{
    public class BookingManagerTests
    {
        [Test]
        public async Task Should_PayForABooking()
        {
            var dto = new PaymentRequestDto
            {
                SelectedPaymentProvider = SupportedPaymentProvider.MercadoPago,
                PaymentIntention = "https://www.mercadopago.com.br/asdf",
                SelectedPaymentMethods = SupportedPaymentMethods.CreditCard
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDto
            {
                CreatedDate = DateTime.Now,
                Message = $"Successfully paid {dto.PaymentIntention}",
                PaymentId = "123",
                Status = PaymentStatus.Success
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Success = true,
                Message = "Payment successfully processed"
            };

            paymentProcessor.
                Setup(x => x.CapturePayment(dto.PaymentIntention))
                .Returns(Task.FromResult(response));

            paymentProcessorFactory
                .Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider))
                .Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                guestRepository.Object,
                roomRepository.Object,
                bookingRepository.Object,
                paymentProcessorFactory.Object);

            var res = await bookingManager.PayForABooking(dto);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.That(res.Message, Is.EqualTo("Payment successfully processed"));
        }
    }
}
