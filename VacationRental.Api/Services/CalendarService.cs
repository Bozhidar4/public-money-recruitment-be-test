using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Api.Core;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;

namespace VacationRental.Api.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHelperService _helperService;

        public CalendarService(
            IBookingRepository bookingRepository,
            IHelperService helperService)
        {
            _bookingRepository = bookingRepository;
            _helperService = helperService;
        }

        public async Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights)
        {
            if (nights < 0)
            {
                throw new ApplicationException(VacationRentalConstants.NightCountErrorMessage);
            }

            await _helperService.CheckRentalExistence(rentalId);

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            var bookings = await _bookingRepository.GetAll();

            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in bookings.Values)
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
