using System.Collections.Generic;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Persistence.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalRepository(IDictionary<int, Rental> rentals)
        {
            _rentals = rentals;
        }

        public Rental Get(int id)
        {
            return _rentals[id];
        }

        public IDictionary<int, Rental> GetAll()
        {
            return _rentals;
        }

        public int Add(Rental rental)
        {
            int id = _rentals.Keys.Count + 1;

            _rentals.Add(id, new Rental
            {
                Id = id,
                Units = rental.Units,
                PreparationTimeInDays = rental.PreparationTimeInDays
            });

            return id;
        }

        public int Update(Rental rental)
        {
            _rentals[rental.Id] = rental;

            return rental.Id;
        }
    }
}
