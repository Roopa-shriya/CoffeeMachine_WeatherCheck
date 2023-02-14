using CoffeeMachine.API.Controllers;
using CoffeeMachine.API.Interfaces;
using CoffeeMachine.API.Models;

namespace CoffeeMachine.API.Services
{
    public class CofffeService : ICofffeService
    {
        private readonly IWeatherService _weatherService;

        public CofffeService( IWeatherService weatherService) 
        {
            _weatherService = weatherService;

        }

        public int CoffeeCount { get; set; }
        public DateTime PreparedTime { get; set; }

        public void UpdateCoffeeCount()
        {
            CoffeeCount++;
            PreparedTime = DateTime.Now;
        }

       public BrewCoffeeResponse GetCoffee()
       {
            BrewCoffeeResponse brewCoffeeResponse = new BrewCoffeeResponse();
            brewCoffeeResponse.prepared = PreparedTime.ToString("yyyy-MM-ddTHH:mm:sszz00");

            double currentTemp = _weatherService.GetCurrentTemperature().Result;

            if (currentTemp > 30)
            {
                brewCoffeeResponse.message = "Your refreshing iced coffee is ready";
            }

            return brewCoffeeResponse;
        }
    }
}
