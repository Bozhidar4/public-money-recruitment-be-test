using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Api.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public IActionResult Get(int rentalId)
        {
            return Ok(_rentalService.Get(rentalId));
        }

        [HttpPost]
        public IActionResult Post(RentalBindingModel model)
        {
            return Ok(_rentalService.Add(model));
        }

        [HttpPut]
        public IActionResult Put(int rentailId, [FromBody] RentalBindingModel model)
        {
            return Ok(_rentalService.Update(rentailId, model));
        }
    }
}
