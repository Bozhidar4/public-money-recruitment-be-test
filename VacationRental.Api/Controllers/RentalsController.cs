using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Get(int rentalId)
        {
            return Ok(await _rentalService.Get(rentalId));
        }

        [HttpPost]
        public async Task<IActionResult> Post(RentalBindingModel model)
        {
            return Ok(await _rentalService.Add(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put(RentalUpdateModel model)
        {
            return Ok(await _rentalService.Update(model));
        }
    }
}
