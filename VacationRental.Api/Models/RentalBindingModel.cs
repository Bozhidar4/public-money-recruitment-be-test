using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Core;

namespace VacationRental.Api.Models
{
    public class RentalBindingModel
    {
        [Range(VacationRentalConstants.MinimumValue, int.MaxValue, 
            ErrorMessage = VacationRentalConstants.UnitCountErrorMessage)]
        public int Units { get; set; }
        [Range(VacationRentalConstants.MinimumValue, int.MaxValue, 
            ErrorMessage = VacationRentalConstants.PreparationDaysCountErrorMessage)]
        public int PreparationTimeInDays { get; set; }
    }
}
