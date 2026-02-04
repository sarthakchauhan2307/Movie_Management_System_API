using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Api.DTo;
using UserService.Api.Model;
using UserService.Api.Services;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Configuration
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IAuthService authService,
            IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }
        #endregion

        #region login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request)
        {
            var user = await _authService.ValidateUserAsync(
                request.Email,
                request.Password
            );

            if (user == null)
                return Unauthorized("Invalid email or password");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    user.UserId,
                    user.UserName,
                    user.Email,
                    user.Role
                }
            });
        }
        #endregion

        #region Generate JWT Token

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "User"),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])
            );

            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser request)
        {
            var user = await _authService.RegisterUserAsync(request);
            if (user == null)
                return BadRequest("User already exists");
            return Ok(new
            {
                user = new
                {
                    user.UserId,
                    user.UserName,
                    user.Email,
                    user.Role
                }
            });
        }
        #endregion
    }
}
