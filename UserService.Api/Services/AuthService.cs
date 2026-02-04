using UserService.Api.DTo;
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

        #region ValidateUserAsync
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }
        #endregion

        #region RegisterUserAsync
        public async Task<User?> RegisterUserAsync(RegisterUser user)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return null; // User already exists
            }
            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                Role = "User" // Default role
            };
            var createdUser = await _userRepository.CreateUserAsync(newUser);
            return createdUser;
        }
        #endregion
    }
}
