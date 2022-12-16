using VacationRental.Domain.Entities.Core;

namespace VacationRental.Domain.Entities.Rentals
{
    public interface IRentalRepository : IRepository<Rental>
    {
        int Add(Rental rental);
        int Update(Rental rental);
    }
}
