using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Domain.Entities.Rentals;

namespace VacationRental.Api.Mappings
{
    public class RentalMapping : Profile
    {
        public RentalMapping()
        {
            CreateMap<Rental, RentalViewModel>();
            CreateMap<RentalBindingModel, Rental>();
        }
    }
}
