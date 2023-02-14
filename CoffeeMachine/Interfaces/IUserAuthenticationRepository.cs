using CoffeeMachine.API.Dtos;
using Microsoft.AspNetCore.Identity;

namespace CoffeeMachine.API.Interfaces
{
    public interface IUserAuthenticationRepository
    {
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();
    }
}
