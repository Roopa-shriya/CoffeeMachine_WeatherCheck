using CoffeeMachine.API.Models;

namespace CoffeeMachine.API.Interfaces
{
    public interface ICofffeService
    {
        int CoffeeCount { get; set; }
        DateTime PreparedTime { get; set; }

        void UpdateCoffeeCount();
        BrewCoffeeResponse GetCoffee();
    }
}
