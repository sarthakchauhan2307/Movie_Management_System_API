using UserService.Api.Model;

namespace UserService.Api.Services
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
    }
}
