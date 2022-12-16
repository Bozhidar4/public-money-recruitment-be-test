using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Domain.Bookings;

namespace VacationRental.Api.Mappings
{
    public class BookingMapping : Profile
    {
        public BookingMapping()
        {
            CreateMap<Booking, BookingViewModel>();
            CreateMap<BookingBindingModel, Booking>();
        }
    }
}
