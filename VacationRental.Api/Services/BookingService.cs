using AutoMapper;
using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IHelperService _helperService;

        public BookingService(
            IMapper mapper,
            IBookingRepository bookingRepository,
            IRentalRepository rentalRepository,
            IHelperService helperService)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _helperService = helperService;
        }

        public BookingViewModel Get(int bookingId)
        {
            if (!GetAllBookings().ContainsKey(bookingId))
            {
                throw new ApplicationException("Booking not found");
            }
             
            return _mapper.Map<BookingViewModel>(_bookingRepository.Get(bookingId));
        }

        public ResourceIdViewModel Add(BookingBindingModel newBooking)
        {
            if (newBooking.Nights <= 0)
            {
                throw new ApplicationException("Nights must be positive");
            }

            _helperService.CheckRentalExistence(newBooking.RentalId);

            var bookings = GetAllBookings();
            var rentals = _rentalRepository.GetAll();

            for (var i = 0; i < newBooking.Nights; i++)
            {
                bool allUnitsAccupied = CheckForAvailableUnits(newBooking, bookings, rentals);

                if (allUnitsAccupied)
                {
                    throw new ApplicationException("Not available");
                }
            }

            var newBookingId = _bookingRepository.Add(_mapper.Map<Booking>(newBooking));

            return new ResourceIdViewModel { Id = newBookingId };
        }

        private IDictionary<int, Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }

        private bool CheckForAvailableUnits(BookingBindingModel newBooking,
                                            IDictionary<int, Booking> bookings,
                                            IDictionary<int, Rental> rentals)
        {
            var count = 0;
            foreach (var booking in bookings.Values)
            {
                var currentBookingDays = booking.Nights + rentals[booking.RentalId].PreparationTimeInDays;
                var newBookingDays = newBooking.Nights + rentals[booking.RentalId].PreparationTimeInDays;

                if (booking.RentalId == newBooking.RentalId
                    && (booking.Start <= newBooking.Start.Date 
                        && booking.Start.AddDays(currentBookingDays) > newBooking.Start.Date)
                    || (booking.Start < newBooking.Start.AddDays(newBookingDays) 
                        && booking.Start.AddDays(currentBookingDays) >= newBooking.Start.AddDays(newBookingDays))
                    || (booking.Start > newBooking.Start 
                        && booking.Start.AddDays(currentBookingDays) < newBooking.Start.AddDays(newBookingDays)))
                {
                    count++;
                }
            }

            return count >= rentals[newBooking.RentalId].Units;
        }
    }
}
