using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IBookingService
    {
        BookingViewModel Get(int bookingId);
        ResourceIdViewModel Add(BookingBindingModel bookingModel);
    }
}
