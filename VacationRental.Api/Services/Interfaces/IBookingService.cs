using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingViewModel> Get(int bookingId);
        Task<ResourceIdViewModel> Add(BookingBindingModel bookingModel);
    }
}
