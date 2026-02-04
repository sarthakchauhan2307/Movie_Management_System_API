using UserService.Api.DTo;
using UserService.Api.Model;

namespace UserService.Api.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> RegisterUserAsync(RegisterUser user);
    }
}
