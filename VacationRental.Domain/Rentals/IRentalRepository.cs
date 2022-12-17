using VacationRental.Domain.Core;

namespace VacationRental.Domain.Rentals
{
    public interface IRentalRepository : IRepository<Rental>
    {
        int Add(Rental rental);
        int Update(Rental rental);
    }
}
