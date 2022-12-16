using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public IActionResult Get(int bookingId)
        {
            return Ok(_bookingService.Get(bookingId));
        }

        [HttpPost]
        public IActionResult Post(BookingBindingModel model)
        {
            return Ok(_bookingService.Add(model));
        }
    }
}
