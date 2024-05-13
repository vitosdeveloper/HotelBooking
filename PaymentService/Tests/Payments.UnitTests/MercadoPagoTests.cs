
using Application.Payment.Ports;
using Application;
using Payment.Application;
using Application.Booking.DTO;

namespace Payments.UnitTests
{
    internal class MercadoPagoTests
    {
        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.MercadoPago);
            Assert.That(typeof(MercadoPagoAdapter), Is.EqualTo(provider.GetType()));
        }

        [Test]
        public async Task Should_FailWhenPaymentIntentionStringIsInvalid()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.MercadoPago);
            var res = await provider.CapturePayment("");
            Assert.False(res.Success);
            Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION));
            Assert.That(res.Message, Is.EqualTo("The selected payment intention is invalid"));
        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.MercadoPago);
            var res = await provider.CapturePayment("https://mercadopago.com.br/asdf");
            Assert.IsTrue(res.Success);
            Assert.AreEqual(res.Message, "Payment successfully processed");
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.CreatedDate);
            Assert.NotNull(res.Data.PaymentId);
        }
    }
}
