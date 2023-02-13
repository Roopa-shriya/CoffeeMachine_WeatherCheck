namespace CoffeeMachine.API.Interfaces
{
    public interface IWeatherService
    {
        Task<double> GetCurrentTemperature();
    }
}
