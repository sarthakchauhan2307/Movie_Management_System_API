using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Api.Data;
using UserService.Api.Model;
using UserService.Api.Services;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        #region configuration
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        #endregion

        #region GetUser
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
           return Ok(await _service.GetUserAsync());
        }
        #endregion

        #region CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
              return Ok( await _service.CreateUserAsync(user));
        }
        #endregion

        #region UpdateUser
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            return Ok( await _service.UpdateUserAsync(id, user));
        }
        #endregion

        #region DeleteUser
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
             return Ok( await _service.DeleteUserAsync(id));
        }
        #endregion

        #region GetUserById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok( await _service.GetUserByIdAsync(id));
        }
        #endregion
    }
}
    