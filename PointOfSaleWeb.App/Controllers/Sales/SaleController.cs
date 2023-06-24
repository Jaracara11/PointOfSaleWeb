using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Sale
{
    [Route("api/sale")]
    [Authorize]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleRepository _salesRepo;

        public SaleController(ISaleRepository salesRepo)
        {
            _salesRepo = salesRepo;
        }

        [HttpGet("discounts")]
        [ResponseCache(Duration = 3600)]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts()
        {
            var discounts = await _salesRepo.GetAllDiscounts();

            if (discounts == null || !discounts.Any())
            {
                return NotFound();
            }

            return Ok(discounts);
        }

        [HttpGet("discounts/{username}")]
        [ResponseCache(Duration = 3600)]
        public async Task<ActionResult<IEnumerable<decimal>>> GetAvailableDiscountsByUsername(string username)
        {
            var discounts = await _salesRepo.GetDiscountsByUsername(username);

            if (discounts == null)
            {
                return NotFound();
            }

            return Ok(discounts);
        }

        [HttpPost("checkout-order")]
        public async Task<ActionResult<Order>> CheckoutOrder(Order order)
        {
            var response = await _salesRepo.NewOrderTransaction(order);

            if (!response.Success)
            {
                ModelState.AddModelError("SalesError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("Sale", response.Data);
        }
    }
}
