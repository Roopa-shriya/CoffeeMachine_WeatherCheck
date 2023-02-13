using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.API.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using CoffeeMachine.API.Interfaces;
using Moq;

namespace CoffeeMachine.API.Test
{
    [TestFixture]
    public class BrewCoffeeUnitTests
    {

        private BrewCoffeeController coffeeController;
        private Mock<ICofffeService> mockCofffeService;
        private Mock<IWeatherService> mockWeatherService;

        [SetUp]
        public void SetUp()
        {
            mockWeatherService = new Mock<IWeatherService>();
            mockCofffeService = new Mock<ICofffeService>();
            coffeeController = new BrewCoffeeController(mockCofffeService.Object, mockWeatherService.Object);
        }

        [Test]
        public void BrewColdCoffee_Returns_200_Ok()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 02, 13, 11, 56, 24));
            mockWeatherService.Setup(x => x.GetCurrentTemperature()).ReturnsAsync(30.5);

            // Act
            var result = coffeeController.BrewCoffee() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            result.Value.Should().BeEquivalentTo(new
            {
                message = "Your refreshing iced coffee is ready",
                prepared = new DateTime(2023, 02, 13, 11, 56, 24).ToString("yyyy-MM-ddTHH:mm:sszz00")
            });

        }

        [Test]
        public void BrewHotCoffee_At_Exact_30Degrees()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 02, 13, 18, 56, 24));
            mockWeatherService.Setup(x => x.GetCurrentTemperature()).ReturnsAsync(30);

            // Act
            var result = coffeeController.BrewCoffee() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            result.Value.Should().BeEquivalentTo(new
            {
                message = "Your piping hot coffee is ready",
                prepared = new DateTime(2023, 02, 13, 18, 56, 24).ToString("yyyy-MM-ddTHH:mm:sszz00")
            });

        }

        [Test]
        public void BrewHotCoffee_Returns_200_Ok()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 04, 13, 11, 56, 24));
            mockWeatherService.Setup(x => x.GetCurrentTemperature()).ReturnsAsync(30);

            // Act
            var result = coffeeController.BrewCoffee() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            result.Value.Should().BeEquivalentTo(new
            {
                message = "Your piping hot coffee is ready",
                prepared = new DateTime(2023, 04, 13, 11, 56, 24).ToString("yyyy-MM-ddTHH:mm:sszz00")
            });

        }

        [Test]
        public void BrewCoffee_Returns_503_ServiceUnavailable()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(5);


            // Act
            var result = coffeeController.BrewCoffee() as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status503ServiceUnavailable);
        }

        [Test]
        public void BrewCoffee_Returns_418_ImATeapot()
        {
            // Arrange
            mockCofffeService.SetupGet(x => x.CoffeeCount).Returns(1);
            mockCofffeService.SetupGet(x => x.PreparedTime).Returns(new DateTime(2023, 04, 1));


            // Act
            var result = coffeeController.BrewCoffee() as StatusCodeResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status418ImATeapot);
        }
    }
}




