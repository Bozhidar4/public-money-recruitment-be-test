using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Bookings;

namespace VacationRental.Persistence.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, Booking> _bookings;

        public BookingRepository(IDictionary<int, Booking> bookings)
        {
            _bookings = bookings;
        }

        public async Task<Booking> Get(int id)
        {
            return await Task.FromResult(_bookings[id]);
        }

        public async Task<IDictionary<int, Booking>> GetAll()
        {
            return await Task.FromResult(_bookings);
        }

        public async Task<int> Add(Booking booking)
        {
            int id = _bookings.Keys.Count + 1;

            _bookings.Add(id, new Booking
            {
                Id = id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start.Date
            });

            return await Task.FromResult(id);
        }
    }
}
