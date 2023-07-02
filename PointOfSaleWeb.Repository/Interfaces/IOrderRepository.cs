using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Discount>> GetAllDiscounts();
        Task<IEnumerable<decimal>> GetDiscountsByUsername(string username);
        Task<OrderDTO> GetOrderByID(string id);
        Task<DbResponse<OrderDTO>> NewOrderTransaction(OrderRequest order);
        Task<IEnumerable<RecentOrderDTO>> GetRecentOrders();
        Task<Decimal> GetTotalSalesOfTheDay();
        Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts();
    }
}
