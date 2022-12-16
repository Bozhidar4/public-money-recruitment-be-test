using VacationRental.Domain.Entities.Core;

namespace VacationRental.Domain.Entities.Rentals
{
    public class Rental : Entity<int>
    {
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
