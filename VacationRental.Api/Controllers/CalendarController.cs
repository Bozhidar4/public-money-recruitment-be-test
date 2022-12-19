using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int rentalId, DateTime start, int nights)
        {
            return Ok(await _calendarService.Get(rentalId, start, nights));
        }
    }
}
