using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Domain.Rentals;

namespace VacationRental.Persistence.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalRepository(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        public async Task<Rental> Get(int id)
        {
            return await Task.FromResult(_rentals[id]);
        }

        public async Task<IDictionary<int, Rental>> GetAll()
        {
            return await Task.FromResult(_rentals);
        }

        public async Task<int> Add(Rental rental)
        {
            int id = _rentals.Keys.Count + 1;

            _rentals.Add(id, new Rental
            {
                Id = id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            });

            return await Task.FromResult(id);
        }

        public async Task<int> Update(Rental rental)
        {
            _rentals[rental.Id] = rental;

            return await Task.FromResult(rental.Id);
        }
    }
}
