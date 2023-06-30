using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Order
{
    [Route("api/order")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _ordersRepo;

        public OrderController(IOrderRepository ordersRepo)
        {
            _ordersRepo = ordersRepo;
        }

        [HttpGet("discounts")]
        [ResponseCache(Duration = 43200)]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts() => Ok(await _ordersRepo.GetAllDiscounts());

        [HttpGet("discounts/{username}")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<decimal>>> GetAvailableDiscountsByUsername(string username) => 
            Ok(await _ordersRepo.GetDiscountsByUsername(username));

        [HttpGet("recent-orders")]
        public async Task<ActionResult<IEnumerable<RecentOrdersDTO>>> GetRecentOrders() => Ok(await _ordersRepo.GetRecentOrders());

        [HttpGet("sales-today")]
        public async Task<ActionResult<Decimal>> GetTotalSalesOfTheDay() => Ok(await _ordersRepo.GetTotalSalesOfTheDay());

        [HttpGet("best-sellers")]
        public async Task<ActionResult<IEnumerable<BestSellerProductDTO>>> GetBestSellerProducts() => Ok(await _ordersRepo.GetBestSellerProducts());

        [HttpPost("checkout-order")]
        public async Task<ActionResult<OrderDTO>> CheckoutOrder(OrderRequest order)
        {
            var response = await _ordersRepo.NewOrderTransaction(order);

            if (!response.Success)
            {
                ModelState.AddModelError("OrdersError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("Order", response.Data);
        }
    }
}
