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

        public BookingService(
            IMapper mapper,
            IBookingRepository bookingRepository,
            IRentalRepository rentalRepository)
        {
            _mapper = mapper;
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public BookingViewModel Get(int bookingId)
        {
            if (!GetAllBookings().ContainsKey(bookingId))
            {
                throw new ApplicationException("Booking not found");
            }
             
            return _mapper.Map<BookingViewModel>(_bookingRepository.Get(bookingId));
        }

        public ResourceIdViewModel Add(BookingBindingModel bookingModel)
        {
            if (bookingModel.Nights <= 0)
            {
                throw new ApplicationException("Nights must be positive");
            }
             
            var rentals = _rentalRepository.GetAll();

            if (!rentals.ContainsKey(bookingModel.RentalId))
            {
                throw new ApplicationException("Rental not found");
            }
                
            var bookings = GetAllBookings();

            for (var i = 0; i < bookingModel.Nights; i++)
            {
                var count = 0;
                foreach (var booking in bookings.Values)
                {
                    if (booking.RentalId == bookingModel.RentalId
                        && (booking.Start <= bookingModel.Start.Date && booking.Start.AddDays(booking.Nights) > bookingModel.Start.Date)
                        || (booking.Start < bookingModel.Start.AddDays(bookingModel.Nights) && booking.Start.AddDays(booking.Nights) >= bookingModel.Start.AddDays(bookingModel.Nights))
                        || (booking.Start > bookingModel.Start && booking.Start.AddDays(booking.Nights) < bookingModel.Start.AddDays(bookingModel.Nights)))
                    {
                        count++;
                    }
                }
                if (count >= rentals[bookingModel.RentalId].Units)
                    throw new ApplicationException("Not available");
            }

            var newBookingId = _bookingRepository.Add(_mapper.Map<Booking>(bookingModel));

            return new ResourceIdViewModel { Id = newBookingId };
        }

        private IDictionary<int, Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }
    }
}
