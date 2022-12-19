using VacationRental.Domain.Core;

namespace VacationRental.Domain.Rentals
{
    public class Rental : Entity<int>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
