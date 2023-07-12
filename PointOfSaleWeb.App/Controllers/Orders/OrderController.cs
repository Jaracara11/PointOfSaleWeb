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
        public async Task<ActionResult<IEnumerable<RecentOrderDTO>>> GetRecentOrders() => Ok(await _ordersRepo.GetRecentOrders());

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderByID(string id)
        {
            var order = await _ordersRepo.GetOrderByID(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("sales-today")]
        public async Task<ActionResult<Decimal>> GetTotalSalesOfTheDay() => Ok(await _ordersRepo.GetTotalSalesOfTheDay());

        [HttpGet("sales-by-date")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<Decimal>> GetSalesByDate(DateTime initialDate, DateTime finalDate) => 
            Ok(await _ordersRepo.GetSalesByDate(initialDate, finalDate));

        [HttpGet("orders-by-date")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<RecentOrderDTO>>> GetOrdersByDate(DateTime initialDate, DateTime finalDate)
        {
            var orders = await _ordersRepo.GetOrdersByDate(initialDate, finalDate);

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }
          
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

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(string id)
        {
            var response = await _ordersRepo.CancelOrder(id);

            if (!response.Success)
            {
                ModelState.AddModelError("OrdersError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
