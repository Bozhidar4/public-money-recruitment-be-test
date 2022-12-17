using System;
using System.Collections.Generic;
using VacationRental.Api.Services.Interfaces;
using VacationRental.Domain.Bookings;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Services
{
    public class HelperService : IHelperService
    {
        private readonly IRentalRepository _rentalRepository;

        public HelperService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public void CheckRentalExistence(int rentalId)
        {
            if (!_rentalRepository.GetAll().ContainsKey(rentalId))
            {
                throw new ApplicationException("Rental not found.");
            }
        }

        public bool CheckForUnitOverlapping(Booking newBooking,
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
