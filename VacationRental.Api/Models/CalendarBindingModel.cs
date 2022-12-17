using System;
using System.ComponentModel.DataAnnotations;
using VacationRental.Api.Core;

namespace VacationRental.Api.Models
{
    public class CalendarBindingModel
    {
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ErrorMessages.NightCountErrorMessage)]
        public int Nights { get; set; }
    }
}
