using Application;
using Application.Booking.DTO;
using Application.Payment.Ports;
using Payment.Application;
using Payments.Application;

namespace Payment.UnitTests
{
    public class Tests
    {
        [Test]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.Stripe);
            Assert.That(typeof(NotImplementedPaymentProvider), Is.EqualTo(provider.GetType()));
        }

        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.MercadoPago);
            Assert.That(typeof(MercadoPagoAdapter), Is.EqualTo(provider.GetType()));
        }

        [Test]
        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();
            var provider = factory.GetPaymentProcessor(SupportedPaymentProvider.Stripe);
            var res = await provider.CapturePayment("https://myprovider.com/asdf");
            Assert.False(res.Success);
            Assert.That(res.ErrorCode, Is.EqualTo(ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED));
            Assert.That(res.Message, Is.EqualTo("The selected payment provider is not available at the moment"));
        }
    }
}