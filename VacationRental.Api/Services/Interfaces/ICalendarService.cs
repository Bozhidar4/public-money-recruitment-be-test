using System;
using System.Threading.Tasks;
using VacationRental.Api.Models;

namespace VacationRental.Api.Services.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarViewModel> Get(int rentalId, DateTime start, int nights);
    }
}
