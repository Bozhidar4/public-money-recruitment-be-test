using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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

        public ResourceIdViewModel Update(int rentalId, RentalBindingModel model)
        {
            _helperService.CheckRentalExistence(rentalId);

            var newBookings = new Dictionary<int, Booking>();
            var rentalBookings = _bookingRepository.GetAll().Where(b => b.Value.RentalId == rentalId);

            var hasConflict = CheckRentalBookingsForConflict(rentalId, model, rentalBookings, newBookings);
            int updatedRentalId = 0;

            if (!hasConflict)
            {
                var rentalMapped = _mapper.Map<Rental>(model);
                rentalMapped.Id = rentalId;
                updatedRentalId = _rentalRepository.Update(rentalMapped);
                rentalBookings = newBookings;
            }
            
            return new ResourceIdViewModel { Id = updatedRentalId };
        }

        private bool CheckRentalBookingsForConflict(int rentalId,
                                                    RentalBindingModel model,
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
