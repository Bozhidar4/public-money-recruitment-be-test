using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IRentalService
    {
        RentalViewModel Get(int rentalId);
        ResourceIdViewModel Add(RentalBindingModel rental);
        ResourceIdViewModel Update(int rentailId, RentalUpdateModel model);
    }
}
