using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
using UserService.Api.Model;

namespace UserService.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Configuration
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetUsersAsync
        public async Task<IEnumerable<User>> GetUserAsync()
        {
            return await _context.Users.ToListAsync();
        }
        #endregion

        #region GetUserByIdAsync
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        #endregion


        #region CreateUserAsync
        public async Task<User> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        #endregion

        #region UpdateUserAsync
        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region DeleteUserAsync
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
