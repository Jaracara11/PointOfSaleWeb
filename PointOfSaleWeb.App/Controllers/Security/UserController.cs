﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PointOfSaleWeb.App.Controllers.Security
{
    [Route("api/user")]
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
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<IEnumerable<UserDataDTO>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<UserDataDTO>> GetUserByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("roles")]
        [ResponseCache(Duration = 3600)]
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
        public async Task<ActionResult<UserDataDTO>> Login(UserLoginDTO user)
        {
            var response = await _userRepo.AuthUser(user);

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
        public async Task<ActionResult<UserDataDTO>> CreateUser(User user)
        {
            var response = await _userRepo.CreateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("User", response.Data);
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(UserDataDTO user)
        {
            var response = await _userRepo.UpdateUser(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangeUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ChangeUserPassword(userData);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpPut("new-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ResetUserPassword(UserChangePasswordDTO userData)
        {
            var response = await _userRepo.ResetUserPassword(userData);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{username}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var response = await _userRepo.DeleteUser(username);

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