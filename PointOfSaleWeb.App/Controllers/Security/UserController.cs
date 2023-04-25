using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PointOfSaleWeb.App.Controllers.Security
{
    [ApiController]
    [Route("api/user")]
    [Authorize]
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
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<IEnumerable<UserInfoDTO>>> GetAllUsersInfo()
        {
            var users = await _userRepo.GetAllUsersInfo();

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("roles")]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllUserRoles()
        {
            var roles = await _userRepo.GetAllUserRoles();

            if (roles == null || !roles.Any())
            {
                return NotFound();
            }

            return Ok(roles);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserInfoDTO>> Login(UserLoginDTO user)
        {
            var response = await _userRepo.GetUserData(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            if (response.Data != null)
            {
                response.Data.Token = CreateToken(response.Data);
            }

            return Ok(response.Data);
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserInfoDTO>> CreateUser(User user)
        {
            var response = await _userRepo.CreateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("User", response.Data);
        }

        [HttpPut("{id}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(int id, UserUpdateDTO user)
        {
            user.UserID = id;

            var response = await _userRepo.UpdateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpPut("{id}/change-password")]
        [Authorize]
        public async Task<ActionResult> ChangeUserPassword(int id, UserChangePasswordDTO user)
        {
            user.UserID = id;

            var response = await _userRepo.ChangeUserPassword(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var response = await _userRepo.DeleteUser(id);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        private string CreateToken(UserInfoDTO user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Jwt:SecretKey").Value ?? ""));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}