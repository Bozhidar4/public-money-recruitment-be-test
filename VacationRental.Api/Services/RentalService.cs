using AutoMapper;
using System;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IMapper _mapper;
        private readonly IRentalRepository _rentalRepository;

        public RentalService(
            IMapper mapper,
            IRentalRepository rentalRepository)
        {
            _mapper = mapper;
            _rentalRepository = rentalRepository;
        }

        public RentalViewModel Get(int rentalId)
        {
            if (!_rentalRepository.GetAll().ContainsKey(rentalId))
            {
                throw new ApplicationException("Rental not found");
            }
                
            return _mapper.Map<RentalViewModel>(_rentalRepository.Get(rentalId));
        }

        public ResourceIdViewModel Add(RentalBindingModel rentalModel)
        {
            var newRentalId = _rentalRepository.Add(_mapper.Map<Rental>(rentalModel));

            return new ResourceIdViewModel { Id = newRentalId };
        }
    }
}
