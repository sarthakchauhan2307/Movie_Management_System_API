using UserService.Api.Model;

namespace UserService.Api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUserAsync();

        Task<User> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync( User user);
        Task<bool> DeleteUserAsync(int id);
    }
}
