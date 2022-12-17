using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Core;

namespace VacationRental.Api.Models
{
    public class RentalBindingModel
    {
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.UnitCountErrorMessage)]
        public int Units { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.PreparationDaysCountErrorMessage)]
        public int PreparationTimeInDays { get; set; }
    }
}
