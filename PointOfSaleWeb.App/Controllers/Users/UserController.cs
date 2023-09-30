using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PointOfSaleWeb.App.Controllers.Users
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDataDTO>>> GetAllUsers() => Ok(await _userRepo.GetAllUsers());

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDataDTO>> GetUserByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);

            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("roles")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllUserRoles() => Ok(await _userRepo.GetAllUserRoles());

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserDataDTO>> Login(UserLoginDTO user)
        {
            var response = await _userRepo.AuthUser(user);

            if (!response.Success)
            {
                return BadRequest(new { error = response.Message });
            }

            if (response.Data != null)
            {
                response.Data.Token = CreateToken(response.Data.Role);
            }

            return Ok(response.Data);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDataDTO>> CreateUser(User user)
        {
            var response = await _userRepo.CreateUser(user);

            return response.Success ? Created("User", response.Data) : BadRequest(new { error = response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(UserDataDTO user)
        {
            var response = await _userRepo.UpdateUser(user);

            return response.Success ? Ok(response) : BadRequest(new { error = response.Message });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangeUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ChangeUserPassword(userData);

            return response.Success ? NoContent() : BadRequest(new { error = response.Message });
        }

        [HttpPut("new-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ResetUserPassword(userData);

            return response.Success ? NoContent() : BadRequest(new { error = response.Message });
        }

        [HttpDelete("{username}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var response = await _userRepo.DeleteUser(username);

            return response.Success ? NoContent() : BadRequest(new { error = response.Message });
        }

        private string CreateToken(string userRole)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, userRole)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:SecretKey").Value ?? ""));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}