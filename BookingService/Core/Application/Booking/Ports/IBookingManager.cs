using Application.Booking.DTO;
using Application.Booking.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDto booking);
        Task<BookingDto> GetBooking(int id);
    }
}
