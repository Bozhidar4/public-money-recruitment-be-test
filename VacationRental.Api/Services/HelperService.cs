using System;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Services
{
    public class HelperService : IHelperService
    {
        private readonly IRentalRepository _rentalRepository;

        public HelperService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public void CheckRentalExistence(int rentalId)
        {
            if (!_rentalRepository.GetAll().ContainsKey(rentalId))
            {
                throw new ApplicationException("Rental not found");
            }
        }
    }
}
