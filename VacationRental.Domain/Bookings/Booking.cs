using System;
using VacationRental.Domain.Core;

namespace VacationRental.Domain.Bookings
{
    public class Booking : Entity<int>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
