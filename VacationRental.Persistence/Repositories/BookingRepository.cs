using System.Collections.Generic;
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

        public Booking Get(int id)
        {
            return _bookings[id];
        }

        public IDictionary<int, Booking> GetAll()
        {
            return _bookings;
        }

        public int Add(Booking booking)
        {
            int id = _bookings.Keys.Count + 1;

            _bookings.Add(id, new Booking
            {
                Id = id,
                Nights = booking.Nights,
                RentalId = booking.RentalId,
                Start = booking.Start.Date
            });

            return id;
        }
    }
}
