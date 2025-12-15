using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
using UserService.Api.Model;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region configuration
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }
        #endregion

        #region GetUser
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = await _context.Users.ToListAsync();
            return Ok(user);
        }
        #endregion

        #region CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User data is null.");
            }
            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }
            user.Role = "User";
            user.Created = DateTime.Now;
            user.Modified = DateTime.Now;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
        #endregion

        #region UpdateUser
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Modified = DateTime.Now;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return Ok(existingUser);
        }
        #endregion

        #region DeleteUser
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound("User not found.");
            }
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully.");
        }
        #endregion
    }
}
