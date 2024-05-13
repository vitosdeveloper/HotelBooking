using Application.Booking.DTO;
using Application.Booking.Ports;
using Application.Booking.Responses;
using Application.Payment.Ports;
using Application.Payment.Responses;
using Domain.DomainExceptions;
using Domain.Ports;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private IGuestRepository _guestRepository;
        private IRoomRepository _roomRepository;
        private IBookingRepository _bookingRepository;
        private IPaymentProcessorFactory _paymentProcessorFactory;

        public BookingManager(
            IGuestRepository guestRepository,
            IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            IPaymentProcessorFactory paymentProcessorFactory
            )
        {
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }

        public async Task<BookingResponse> CreateBooking(BookingDto bookingDto)
        {
            try
            {
                var booking = BookingDto.MapToEntity(bookingDto);
                booking.Guest = await _guestRepository.Get(bookingDto.GuestId);
                booking.Room = await _roomRepository.GetAggregate(bookingDto.RoomId);
                await booking.Save(_bookingRepository);
                bookingDto.Id = booking.Id;
                return new BookingResponse
                {
                    Success = true,
                    Data = bookingDto,
                };
            }
            catch (PlacedAtIsRequiredInformationException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "PlacedAt is a required information"
                };
            }
            catch (StartDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "StartDate is a required information"
                };
            }
            catch (EndDateTimeIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "EndDate is a required information"
                };
            }
            catch (RoomIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Room is a required information"
                };
            }
            catch (GuestIsRequiredException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION,
                    Message = "Guest is a required information"
                };
            }
            catch (RoomCannotBeBookedException)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED,
                    Message = "This room can't be booked."
                };
            }
            catch (Exception)
            {
                return new BookingResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.BOOKING_COULD_NOT_STORE_DATA,
                    Message = "There was an error saving to the DB"
                };
            }
        }
        public async Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto)
        {
            var paymentProcessor = _paymentProcessorFactory.GetPaymentProcessor(paymentRequestDto.SelectedPaymentProvider);
            var response = await paymentProcessor.CapturePayment(paymentRequestDto.PaymentIntention);
            if (response.Success)
            {
                return new PaymentResponse
                {
                    Success = true,
                    Data = response.Data,
                    Message = "Payment successfully processed"
                };
            }
            return response;
        }

        public async Task<BookingResponse> GetBooking(int id)
        {
            throw new NotImplementedException();
        }
    }
}
