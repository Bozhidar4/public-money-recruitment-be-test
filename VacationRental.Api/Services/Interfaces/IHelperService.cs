using System.Collections.Generic;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IHelperService
    {
        void CheckRentalExistence(int rentalId);
        bool CheckForUnitOverlapping(Booking newBooking,
                                     IDictionary<int, Booking> bookings,
                                     IDictionary<int, Rental> rentals);
    }
}
