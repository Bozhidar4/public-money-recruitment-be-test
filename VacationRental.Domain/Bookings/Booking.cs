using System;
using VacationRental.Domain.Entities.Core;

namespace VacationRental.Domain.Bookings
{
    public class Booking : Entity<int>
    {
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
    }
}
