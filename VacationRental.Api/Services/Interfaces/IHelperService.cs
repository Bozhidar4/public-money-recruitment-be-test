using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IHelperService
    {
        Task CheckRentalExistence(int rentalId);
        bool CheckForUnitOverlapping(Booking newBooking,
                                     IDictionary<int, Booking> bookings,
                                     IDictionary<int, Rental> rentals);
    }
}
