using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<BookingViewModel> Get(int bookingId)
        {
            var bookings = await GetAllBookings();

            if (!bookings.ContainsKey(bookingId))
            {
                throw new ApplicationException(VacationRentalConstants.BookingNotFoundErrorMessage);
            }
             
            return _mapper.Map<BookingViewModel>(await _bookingRepository.Get(bookingId));
        }

        public async Task<ResourceIdViewModel> Add(BookingBindingModel newBooking)
        {
            await _helperService.CheckRentalExistence(newBooking.RentalId);

            var bookings = await GetAllBookings();
            var rentals = await _rentalRepository.GetAll();
            var newBookingMapped = _mapper.Map<Booking>(newBooking);

            for (var i = 0; i < newBooking.Nights; i++)
            {
                bool allUnitsOccupied = _helperService.CheckForUnitOverlapping(newBookingMapped, bookings, rentals);

                if (allUnitsOccupied)
                {
                    throw new ApplicationException(VacationRentalConstants.NotAvailableErrorMessage);
                }
            }

            var newBookingId = await _bookingRepository.Add(newBookingMapped);

            return new ResourceIdViewModel { Id = newBookingId };
        }

        private async Task<IDictionary<int, Booking>> GetAllBookings()
        {
            return await _bookingRepository.GetAll();
        } 
    }
}
