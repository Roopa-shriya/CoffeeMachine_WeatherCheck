using CoffeeMachine.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CoffeeMachine.API.Dtos;

namespace CoffeeMachine.API.Controllers
{
    [Route("/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IUserAuthenticationRepository _repository;
        public LoginController(IUserAuthenticationRepository repository) 
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto user)
        {
            return !await _repository.ValidateUserAsync(user)
            ? Unauthorized()
                : Ok(await _repository.CreateTokenAsync());
        }
    }
}
