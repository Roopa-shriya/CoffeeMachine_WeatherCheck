using CoffeeMachine.API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.API.Controllers
{
    [Route("/brew-coffee")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly ICofffeService _coffeeService;

        public BrewCoffeeController(ICofffeService coffeeService)
        {
            _coffeeService= coffeeService;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult BrewCoffee()
        {
            _coffeeService.UpdateCoffeeCount();

            if (_coffeeService.PreparedTime.Month == 4 && _coffeeService.PreparedTime.Day == 1)
            {
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            if (_coffeeService.CoffeeCount % 5 == 0)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable);
            }
            
            return Ok(_coffeeService.GetCoffee());
            
        }        
    }
}
