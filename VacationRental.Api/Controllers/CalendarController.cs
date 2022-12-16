using System;
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
        public IActionResult Get(int rentalId, DateTime start, int nights)
        {
            return Ok(_calendarService.Get(rentalId, start, nights));
        }
    }
}
