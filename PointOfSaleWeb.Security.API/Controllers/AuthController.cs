using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.Security.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public AuthController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        public async Task<ActionResult<UserInfoDTO>> Login(UserLoginDTO user)
        {
            var response = await _userRepo.GetUserData(user);

            if (!response.Success)
            {
                ModelState.AddModelError("UserError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response.Data);
        }
    }
}