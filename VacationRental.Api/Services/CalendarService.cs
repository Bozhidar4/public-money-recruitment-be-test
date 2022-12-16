using System;
using System.Collections.Generic;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(
            IBookingRepository bookingRepository,
            IRentalRepository rentalRepository)
        {
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
        }

        public CalendarViewModel Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
            {
                throw new ApplicationException("Nights must be positive");
            }  
            if (!_rentalRepository.GetAll().ContainsKey(rentalId))
            {
                throw new ApplicationException("Rental not found");
            }
               
            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in _bookingRepository.GetAll().Values)
                {
                    if (booking.RentalId == rentalId
                        && booking.Start <= date.Date && booking.Start.AddDays(booking.Nights) > date.Date)
                    {
                        date.Bookings.Add(new CalendarBookingViewModel
                        {
                            Id = booking.Id,
                            Unit = booking.RentalId
                        });

                        date.PreparationTimes.Add(new PreparationTimeViewModel
                        {
                            Unit = booking.RentalId
                        });
                    }
                }

                result.Dates.Add(date);
            }

            return result;
        }
    }
}
