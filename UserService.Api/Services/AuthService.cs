using UserService.Api.Model;
using UserService.Api.Repositories;

namespace UserService.Api.Services
{
    public class AuthService : IAuthService
    {
        #region Configuration
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }
    }
}
