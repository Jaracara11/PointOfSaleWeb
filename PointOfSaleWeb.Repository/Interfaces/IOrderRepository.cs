using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<IEnumerable<decimal>> GetDiscountsByUsername(string username);
        Task<DbResponse<OrderDTO>> NewOrderTransaction(OrderRequest order);
        Task<IEnumerable<RecentOrdersDTO>> GetRecentOrders();
        Task<Decimal> GetTotalSalesOfTheDay();
    }
}
