using System.Collections.Generic;
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

        public CalendarViewModel Get(CalendarBindingModel model)
        {
            _helperService.CheckRentalExistence(model.RentalId);

            var result = new CalendarViewModel
            {
                RentalId = model.RentalId,
                Dates = new List<CalendarDateViewModel>()
            };
            for (var i = 0; i < model.Nights; i++)
            {
                var date = new CalendarDateViewModel
                {
                    Date = model.StartDate.Date.AddDays(i),
                    Bookings = new List<CalendarBookingViewModel>(),
                    PreparationTimes = new List<PreparationTimeViewModel>()
                };

                foreach (var booking in _bookingRepository.GetAll().Values)
                {
                    if (booking.RentalId == model.RentalId
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
