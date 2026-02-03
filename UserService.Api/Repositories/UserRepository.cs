using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using UserService.Api.Data;
using UserService.Api.Model;

namespace UserService.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Configuration
        private readonly DapperContext _context;
        private readonly IMemoryCache _cache; 

        public UserRepository(DapperContext context , IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        #endregion

        #region GetUsersAsync
        public async Task<IEnumerable<User>> GetUserAsync()
        {
            const string cacheKey = "all_users";

            //  Check cache
            if (_cache.TryGetValue(cacheKey, out IEnumerable<User> cachedUsers))
            {
                return cachedUsers;
            }

            //  Call DB using Dapper + SP
            using var connection = _context.CreateConnection();

            var users = await connection.QueryAsync<User>(
                "SP_GetUsers",
                commandType: CommandType.StoredProcedure
            );

            // store result in cache (5 minutes)
            _cache.Set(cacheKey, users, TimeSpan.FromMinutes(5));

            return users;
        }
        #endregion

        #region GetUserByIdAsync
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            string cacheKey = $"user_{userId}";

            // 1️ Check cache
            if (_cache.TryGetValue(cacheKey, out User cachedUser))
            {
                return cachedUser;
            }

            // 2️ Call DB using Dapper + SP
            using var connection = _context.CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SP_GetUserById",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            // 3️ Store in cache
            if (user != null)
            {
                _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
            }

            return user;
        }
        #endregion

        #region CreateUserAsync
        public async Task<User> CreateUserAsync(User user)
        {
            using var connection = _context.CreateConnection();

            var userId = await connection.QuerySingleAsync<int>
                ("SP_CreateUser",
                new
                {
                    user.UserName,
                    user.Email,
                    user.Password,
                    user.PhoneNumber,
                    //user.Role
                    //user.Created,
                    //user.Modified
                },
                  commandType: CommandType.StoredProcedure);

            user.UserId = userId;
            return user;
        }
        #endregion

        #region UpdateUserAsync
        public async Task<bool> UpdateUserAsync(User user)
        {
            using var connection = _context.CreateConnection();

            var rowsAffected = await connection.ExecuteScalarAsync<int>(
                "SP_UpdateUser",
                new
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                },
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        #endregion

        #region DeleteUserAsync
        public async Task<bool> DeleteUserAsync(int id)
        {
            using var connection = _context.CreateConnection();

            var affectedRows = await connection.ExecuteAsync
                ("SP_DeleteUser",
                new { UserId = id },
                commandType: CommandType.StoredProcedure);
            return affectedRows > 0;
        }
        #endregion

        #region GetByEmailAsync
        public async Task<User?> GetByEmailAsync(string email)
        {
            string cacheKey = $"user_email_{email}";

            // 1️⃣ Check cache
            if (_cache.TryGetValue(cacheKey, out User cachedUser))
            {
                return cachedUser;
            }

            // 2️⃣ Call DB using Dapper + SP
            using var connection = _context.CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SP_GetUserByEmail",   // Stored Procedure
                new { Email = email },
                commandType: CommandType.StoredProcedure
            );

            // 3️⃣ Store in cache
            if (user != null)
            {
                _cache.Set(cacheKey, user, TimeSpan.FromMinutes(5));
            }

            return user;
        }
        #endregion


    }
}
