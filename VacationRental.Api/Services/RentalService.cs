using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

namespace VacationRental.Api.Services
{
    public class RentalService : IRentalService
    {
        private readonly IMapper _mapper;
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IHelperService _helperService;

        public RentalService(
            IMapper mapper,
            IRentalRepository rentalRepository,
            IBookingRepository bookingRepository,
            IHelperService helperService)
        {
            _mapper = mapper;
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _helperService = helperService;
        }

        public async Task<RentalViewModel> Get(int rentalId)
        {
            await _helperService.CheckRentalExistence(rentalId);

            return _mapper.Map<RentalViewModel>(await _rentalRepository.Get(rentalId));
        }

        public async Task<ResourceIdViewModel> Add(RentalBindingModel rentalModel)
        {
            var newRentalId = await _rentalRepository.Add(_mapper.Map<Rental>(rentalModel));

            return new ResourceIdViewModel { Id = newRentalId };
        }

        public async Task<ResourceIdViewModel> Update(RentalUpdateModel model)
        {
            await _helperService.CheckRentalExistence(model.Id);

            var newBookings = new Dictionary<int, Booking>();
            var bookings = await _bookingRepository.GetAll();
            var rentalBookings = bookings.Where(b => b.Value.RentalId == model.Id);

            var hasConflict = CheckRentalBookingsForConflict(model.Id, model, rentalBookings, newBookings);

            if (hasConflict)
            {
                throw new ApplicationException("There is overlapping, the update cannot be performed.");
            }

            var rentalMapped = _mapper.Map<Rental>(model);
            rentalMapped.Id = model.Id;
            var updatedRentalId = await _rentalRepository.Update(rentalMapped);
            rentalBookings = newBookings;

            return new ResourceIdViewModel { Id = updatedRentalId };
        }

        private bool CheckRentalBookingsForConflict(int rentalId,
                                                    RentalUpdateModel model,
                                                    IEnumerable<KeyValuePair<int, Booking>> rentalBookings,
                                                    Dictionary<int, Booking> newBookings)
        {
            var updatedRental = new Rental
            {
                Id = rentalId,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };
            var updatedRentals = new Dictionary<int, Rental>
            {
                { rentalId, updatedRental }
            };

            bool bookingsHaveConflict = false;

            foreach (var booking in rentalBookings)
            {
                var bookingHasConflict = _helperService.CheckForUnitOverlapping(booking.Value, newBookings, updatedRentals);

                if (!bookingHasConflict)
                {
                    int index = newBookings.Count + 1;
                    newBookings.Add(index, booking.Value);
                }
                else
                {
                    bookingsHaveConflict = true;
                }
            }

            return bookingsHaveConflict;
        }
    }
}
