using System.Threading.Tasks;
using VacationRental.Domain.Core;

namespace VacationRental.Domain.Bookings
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<int> Add(Booking booking);
    }
}
