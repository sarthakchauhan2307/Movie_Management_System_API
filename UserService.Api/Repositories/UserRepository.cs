using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserService.Api.Data;
using UserService.Api.Model;

namespace UserService.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Configuration
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }
        #endregion

        #region GetUsersAsync
        public async Task<IEnumerable<User>> GetUserAsync()
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<User>("SP_GetUsers", commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region GetUserByIdAsync
        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<User>
                ("SP_GetUserById",
                new { UserId = id },
                commandType: CommandType.StoredProcedure);
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
    }
}
