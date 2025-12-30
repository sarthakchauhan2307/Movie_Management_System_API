using UserService.Api.Model;
using UserService.Api.Repositories;

namespace UserService.Api.Services
{
    public class UserServices : IUserService
    {
        #region Configuration
        private readonly IUserRepository _userRepository;
        private readonly MicroServiceGateway _gateway;

        public UserServices(IUserRepository userRepository, MicroServiceGateway gateway)
        {
            _userRepository = userRepository;
            _gateway = gateway;
        }
        #endregion

        #region GetAllUsers
        public async Task<IEnumerable<User>> GetUserAsync()
        {
            return await _userRepository.GetUserAsync();
        }
        #endregion

        #region GetUserById
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return user;
        }
        #endregion

        #region CreateUser
        public async Task<User> CreateUserAsync(User user)
        {
            user.Role = "User";
            user.Created = DateTime.Now;
            user.Modified = DateTime.Now;
            return await _userRepository.CreateUserAsync(user);
        }
        #endregion

        #region UpdateUser
        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Password = user.Password;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Modified = DateTime.Now;
            return await _userRepository.UpdateUserAsync(existingUser);
        }
        #endregion

        #region DeleteUser
        public async Task<bool> DeleteUserAsync(int id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found");
            var hasBookings = await _gateway.HasBooking(id);
            if (hasBookings)
                throw new InvalidOperationException("Cannot delete user with existing bookings");
            return await _userRepository.DeleteUserAsync(id);
        }
        #endregion

    }
}
