using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PointOfSaleWeb.App.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserRepository userRepo, IConfiguration configuration) : ControllerBase
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IConfiguration _configuration = configuration;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<IEnumerable<UserDataDTO>>> GetAllUsers() => Ok(await _userRepo.GetAllUsers());

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<UserDataDTO>> GetUserByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);

            return user != null ? Ok(user) : NotFound();
        }

        [HttpGet("roles"), AllowAnonymous]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllUserRoles() => Ok(await _userRepo.GetAllUserRoles());

        [HttpPost("auth"), AllowAnonymous]
        public async Task<ActionResult<UserInfoDTO>> AuthUser(UserAuthDTO user)
        {
            var response = await _userRepo.AuthUser(user);

            if (!response.Success)
            {
                return BadRequest(new { response.Message });
            }

            var userData = response.Data;

            if (userData != null)
            {
                var userInfo = new UserInfoDTO
                {
                    Username = userData.Username,
                    Name = $"{userData.FirstName} {userData.LastName}",
                    Email = userData.Email,
                    Role = userData.RoleName,
                    Token = CreateToken(userData.RoleName)
                };

                return Ok(userInfo);
            }

            return BadRequest(new { Message = "An unknown error occurred." });
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDataDTO>> CreateUser(User user)
        {
            var response = await _userRepo.CreateUser(user);

            return response.Success ? Created("User", response.Data) : BadRequest(new { response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(UserDataDTO user)
        {
            var response = await _userRepo.UpdateUser(user);

            return response.Success ? Ok(response) : BadRequest(new { response.Message });
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangeUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ChangeUserPassword(userData);

            return response.Success ? NoContent() : BadRequest(new { response.Message });
        }

        [HttpPut("new-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ResetUserPassword(userData);

            return response.Success ? NoContent() : BadRequest(new { response.Message });
        }

        [HttpDelete("{username}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var response = await _userRepo.DeleteUser(username);

            return response.Success ? NoContent() : BadRequest(new { response.Message });
        }

        private static string CreateToken(string userRole)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Role, userRole)
            ];

            var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.
                       GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? ""));

            var credentials = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}