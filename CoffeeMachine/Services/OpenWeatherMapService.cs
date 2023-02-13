using CoffeeMachine.API.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace CoffeeMachine.API.Services
{
    public class OpenWeatherMapService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        public OpenWeatherMapService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<double> GetCurrentTemperature()
        {
            double? tempInCelsius = null;
            const double convertFromKelvin = 273.15;

            WeatherApiSettings weatherApi = _configuration.GetSection("WeatherAPI").Get<WeatherApiSettings>();

            var queryStingValues = new Dictionary<string, string>
            {
                ["lat"] = weatherApi.Latitude,
                ["lon"] = weatherApi.Longitude,
                ["appid"] = weatherApi.ApiKey
            };

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri(QueryHelpers.AddQueryString(weatherApi.ApiEndpoint, queryStingValues));
            request.Method = HttpMethod.Get;
           // request.Headers.Add("api_key", Environment.GetEnvironmentVariable("DPIRD_APIKEY"));
           
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                var response = await responseMessage.Content.ReadFromJsonAsync<CurrentWeatherResponse>();                
                if (response.main != null)
                {
                    tempInCelsius = response.main.temp - convertFromKelvin;
                }
            }

            return tempInCelsius.Value;
        }

        private class WeatherApiSettings
        {
            public string ApiEndpoint { get; set; }
            public string ApiKey { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }

        }

        private class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
        }

        private class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        private class CurrentWeatherResponse
        {
            public object coord { get; set; }
            public List<object> weather { get; set; }
            public string @base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public object wind { get; set; }
            public object clouds { get; set; }
            public int dt { get; set; }
            public object sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }

        }

    }
}

