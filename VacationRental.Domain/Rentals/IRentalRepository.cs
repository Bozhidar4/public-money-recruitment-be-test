using System.Threading.Tasks;
using VacationRental.Domain.Core;

namespace VacationRental.Domain.Rentals
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<int> Add(Rental rental);
        Task<int> Update(Rental rental);
    }
}
