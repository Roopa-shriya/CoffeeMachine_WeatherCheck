using System.Net;
using CoffeeMachine.API.Controllers;
using CoffeeMachine.API.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;

namespace CoffeeMachine.API.Test
{
    [TestFixture]
    public class BrewCoffeeIntegrationTests
    {
        private HttpClient _client;
        private Mock<IWeatherService> mockWeatherService;

        [SetUp]
        public void Setup()
        {
            
            mockWeatherService = new Mock<IWeatherService>();

            var factory = new WebApplicationFactory<BrewCoffeeController>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddSingleton(mockWeatherService.Object);
                    });
                });
            _client = factory.CreateDefaultClient();
            
        }

        [Test]
        public async Task BrewHotCoffee_Returns200Ok()
        {
            // Arrange
            if (!(DateTime.Now.Month == 4 && DateTime.Now.Day == 1))
            {
                mockWeatherService.Setup(x => x.GetCurrentTemperature()).ReturnsAsync(20);

                // Act
                var response = await _client.GetAsync("/brew-coffee");

                // Assert
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseString);

                responseJson["message"].Value<string>().Should().Be("Your piping hot coffee is ready");
                responseJson["prepared"].Value<string>().Should().NotBeNullOrWhiteSpace();
            }
        }

        [Test]
        public async Task BrewColdCoffee_Returns200Ok()
        {
            // Arrange
            if (!(DateTime.Now.Month == 4 && DateTime.Now.Day == 1))
            {
                mockWeatherService.Setup(x => x.GetCurrentTemperature()).ReturnsAsync(32);

                // Act
                var response = await _client.GetAsync("/brew-coffee");

                // Assert
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JObject.Parse(responseString);

                responseJson["message"].Value<string>().Should().Be("Your refreshing iced coffee is ready");
                responseJson["prepared"].Value<string>().Should().NotBeNullOrWhiteSpace();
            }
        }


        [Test]
        public async Task BrewCoffee_Returns503ServiceUnavailable()
        {
            // Arrange
            if (!(DateTime.Now.Month == 4 && DateTime.Now.Day == 1))
            {
                for (int i = 0; i < 4; i++)
                {
                    var response = await _client.GetAsync("/brew-coffee");
                    response.EnsureSuccessStatusCode();
                }

                // Act
                var finalResponse = await _client.GetAsync("/brew-coffee");

                // Assert
                finalResponse.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
            }
        }

        //This test will be valid only for April 1st
        [Test]
        public async Task BrewCoffee_Returns418ImATeapot()
        {
            // Arrange
            if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
            {
                // Act
                var response = await _client.GetAsync("/brew-coffee");

                // Assert
                response.StatusCode.Should().Be((HttpStatusCode)418);
            }
        }
    }
}