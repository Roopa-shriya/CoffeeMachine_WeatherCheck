using CoffeeMachine.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.API.Controllers
{
    [Route("/brew-coffee")]
    [ApiController]
    public class BrewCoffeeController : ControllerBase
    {
        private readonly ICofffeService _coffeeService;
        private readonly IWeatherService _weatherService;

        public BrewCoffeeController(ICofffeService coffeeService, IWeatherService weatherService)
        {
            _coffeeService= coffeeService;
            _weatherService = weatherService;
        }


        [HttpGet]
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

            BrewCoffeeResponse brewCoffeeResponse = new BrewCoffeeResponse();
            brewCoffeeResponse.prepared = _coffeeService.PreparedTime.ToString("yyyy-MM-ddTHH:mm:sszz00");

            double currentTemp = _weatherService.GetCurrentTemperature().Result;

            if (currentTemp > 30)
            {
                brewCoffeeResponse.message = "Your refreshing iced coffee is ready";                    
            }

            return Ok(brewCoffeeResponse);
        }

        private class BrewCoffeeResponse
        {
            public string message { get; set; } = "Your piping hot coffee is ready";
            public string prepared { get; set; }
        }
    }
}
