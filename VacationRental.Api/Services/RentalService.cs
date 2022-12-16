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
        private readonly IHelperService _helperService;

        public RentalService(
            IMapper mapper,
            IRentalRepository rentalRepository,
            IHelperService helperService)
        {
            _mapper = mapper;
            _rentalRepository = rentalRepository;
            _helperService = helperService;
        }

        public RentalViewModel Get(int rentalId)
        {
            _helperService.CheckRentalExistence(rentalId);

            return _mapper.Map<RentalViewModel>(_rentalRepository.Get(rentalId));
        }

        public ResourceIdViewModel Add(RentalBindingModel rentalModel)
        {
            var newRentalId = _rentalRepository.Add(_mapper.Map<Rental>(rentalModel));

            return new ResourceIdViewModel { Id = newRentalId };
        }

        public ResourceIdViewModel Update(int rentalId, RentalUpdateModel model)
        {
            _helperService.CheckRentalExistence(rentalId);

            var rentalMapped = _mapper.Map<Rental>(model);
            rentalMapped.Id = rentalId;

            return new ResourceIdViewModel { Id = _rentalRepository.Update(rentalMapped) };
        }
    }
}
