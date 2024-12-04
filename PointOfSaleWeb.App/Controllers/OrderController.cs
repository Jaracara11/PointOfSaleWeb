using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController(IOrderRepository ordersRepo) : ControllerBase
    {
        private readonly IOrderRepository _ordersRepo = ordersRepo;

        [HttpGet("discounts")]
        [ResponseCache(Duration = 43200)]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts() => Ok(await _ordersRepo.GetAllDiscounts());

        [HttpGet("discounts/{username}")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<decimal>>> GetAvailableDiscountsByUsername(string username) =>
            Ok(await _ordersRepo.GetDiscountsByUsername(username));

        [HttpGet("recent-orders")]
        [ResponseCache(Duration = 5)]
        // public async Task<ActionResult<IEnumerable<RecentOrderDTO>>> GetRecentOrders() => Ok(await _ordersRepo.GetRecentOrders());
        public async Task<ActionResult<IEnumerable<RecentOrderDTO>>> GetRecentOrders() => Ok(new List<RecentOrderDTO>());

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<OrderDTO>> GetOrderByID(string id)
        {
            var order = await _ordersRepo.GetOrderByID(id);

            return order != null ? Ok(order) : NotFound();
        }

        [HttpGet("sales-today")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<decimal>> GetTotalSalesOfTheDay() => Ok(await _ordersRepo.GetTotalSalesOfTheDay());

        [HttpGet("sales-by-date")]
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<decimal>> GetSalesByDate(DateTime? initialDate, DateTime? finalDate)
        {
            var dateValidationResult = ValidationUtil.DateRangeValidation(initialDate, finalDate);

            return dateValidationResult.Success ? Ok(await _ordersRepo.GetSalesByDate(initialDate!.Value, finalDate!.Value))
                : BadRequest(new { dateValidationResult.Message });
        }

        [HttpGet("orders-by-date")]
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IEnumerable<RecentOrderDTO>>> GetOrdersByDate(DateTime? initialDate, DateTime? finalDate)
        {
            var dateValidationResult = ValidationUtil.DateRangeValidation(initialDate, finalDate);

            return dateValidationResult.Success ? Ok(await _ordersRepo.GetOrdersByDate(initialDate!.Value, finalDate!.Value))
                : BadRequest(new { dateValidationResult.Message });
        }

        [HttpPost("checkout-order")]
        public async Task<ActionResult<OrderDTO>> CheckoutOrder(OrderRequest order)
        {
            var response = await _ordersRepo.NewOrderTransaction(order);

            return response.Success ? Created("Order", response.Data) : BadRequest(new { response.Message });
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(string id)
        {
            var response = await _ordersRepo.CancelOrder(id);

            return response.Success ? NoContent() : BadRequest(new { response.Message });
        }
    }
}
