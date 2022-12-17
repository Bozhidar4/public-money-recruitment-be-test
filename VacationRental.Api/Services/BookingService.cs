using AutoMapper;
using System;
using System.Collections.Generic;
using VacationRental.Api.Core;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Rentals;

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
                throw new ApplicationException(VacationRentalConstants.BookingNotFoundErrorMessage);
            }
             
            return _mapper.Map<BookingViewModel>(_bookingRepository.Get(bookingId));
        }

        public ResourceIdViewModel Add(BookingBindingModel newBooking)
        {
            _helperService.CheckRentalExistence(newBooking.RentalId);

            var bookings = GetAllBookings();
            var rentals = _rentalRepository.GetAll();
            var newBookingMapped = _mapper.Map<Booking>(newBooking);

            for (var i = 0; i < newBooking.Nights; i++)
            {
                bool allUnitsAccupied = _helperService.CheckForUnitOverlapping(newBookingMapped, bookings, rentals);

                if (allUnitsAccupied)
                {
                    throw new ApplicationException(VacationRentalConstants.NotAvailableErrorMessage);
                }
            }

            var newBookingId = _bookingRepository.Add(newBookingMapped);

            return new ResourceIdViewModel { Id = newBookingId };
        }

        private IDictionary<int, Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        } 
    }
}
