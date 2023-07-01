using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;
using System.Text.Json;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly DbContext _context;

        public OrderRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscounts()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<Discount>("GetAllDiscounts", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<decimal>> GetDiscountsByUsername(string username)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            return await db.QueryAsync<decimal>("GetDiscountsByUsername", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<RecentOrderDTO>> GetRecentOrders()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<RecentOrderDTO>("GetRecentOrders", commandType: CommandType.StoredProcedure);
        }

        public async Task<Decimal> GetTotalSalesOfTheDay()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QuerySingleOrDefaultAsync<Decimal>("GetTotalSalesOfTheDay", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<BestSellerProductDTO>("GetBestSellerProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<DbResponse<OrderDTO>> NewOrderTransaction(OrderRequest order)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@User", order.User);
            parameters.Add("@Products", JsonSerializer.Serialize(order.Products));
            parameters.Add("@Discount", order.Discount);

            try
            {
                var orderResponse = await db.QuerySingleOrDefaultAsync<OrderDTO>("NewOrderTransaction", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<OrderDTO>
                {
                    Success = true,
                    Data = orderResponse
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<OrderDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
