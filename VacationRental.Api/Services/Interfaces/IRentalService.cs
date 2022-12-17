using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface IRentalService
    {
        Task<RentalViewModel> Get(int rentalId);
        Task<ResourceIdViewModel> Add(RentalBindingModel rental);
        Task<ResourceIdViewModel> Update(RentalUpdateModel model);
    }
}
