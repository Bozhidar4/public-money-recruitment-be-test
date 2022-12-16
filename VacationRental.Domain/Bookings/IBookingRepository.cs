using VacationRental.Domain.Entities.Core;

namespace VacationRental.Domain.Bookings
{
    public interface IBookingRepository : IRepository<Booking>
    {
        int Add(Booking booking);
    }
}
