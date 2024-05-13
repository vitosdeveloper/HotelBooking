namespace Application.Booking.DTO
{
    public enum SupportedPaymentProvider
    {
        Paypal = 1,
        Stripe = 2,
        PagSeguro = 3,
        MercadoPago = 4,
    }

    public enum SupportedPaymentMethods
    {
        DebitCard = 1,
        CreditCard = 2,
        BankTransfer = 3,
    }
    public class PaymentRequestDto
    {
        public int BookingId { get; set; }
        public string PaymentIntention { get; set; }
        public SupportedPaymentProvider SelectedPaymentProvider { get; set; }
        public SupportedPaymentMethods SelectedPaymentMethods { get; set; }
    }
}
