using Application.Booking.DTO;
using Payment.Application;
using Payments.Application;

namespace Application.Payment.Ports
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {
        public IPaymentProcessor GetPaymentProcessor(SupportedPaymentProvider selectedPaymentProvider)
        {
            switch (selectedPaymentProvider)
            {
                case SupportedPaymentProvider.MercadoPago:
                    return new MercadoPagoAdapter();

                default: return new NotImplementedPaymentProvider();
            }
        }
    }
}
