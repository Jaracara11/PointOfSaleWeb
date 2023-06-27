using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
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
        [ResponseCache(Duration = 43200)]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts() => Ok(await _salesRepo.GetAllDiscounts());

        [HttpGet("discounts/{username}")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<decimal>>> GetAvailableDiscountsByUsername(string username) => 
            Ok(await _salesRepo.GetDiscountsByUsername(username));

        [HttpPost("checkout-order")]
        public async Task<ActionResult<OrderDTO>> CheckoutOrder(OrderRequest order)
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
