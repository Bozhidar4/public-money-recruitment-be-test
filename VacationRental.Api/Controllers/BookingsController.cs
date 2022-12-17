using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get(int bookingId)
        {
            return Ok(await _bookingService.Get(bookingId));
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookingBindingModel model)
        {
            return Ok(await _bookingService.Add(model));
        }
    }
}
