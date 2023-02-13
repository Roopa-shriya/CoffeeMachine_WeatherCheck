using CoffeeMachine.API.Interfaces;

namespace CoffeeMachine.API.Services
{
    public class CofffeService : ICofffeService
    {
        public int CoffeeCount { get; set; }
        public DateTime PreparedTime { get; set; }

        public void UpdateCoffeeCount()
        {
            CoffeeCount++;
            PreparedTime = DateTime.Now;
        }        
    }
}
