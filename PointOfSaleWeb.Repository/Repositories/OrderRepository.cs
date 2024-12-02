using Dapper;
using MySql.Data.MySqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;
using System.Text.Json;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class OrderRepository : IOrderRepository
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
            return await db.QueryAsync<decimal>("GetDiscountsByUsername", new { Username = username }, commandType: CommandType.StoredProcedure);
        }

        public async Task<OrderDTO?> GetOrderByID(string id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<OrderDTO>("GetOrderById", new { OrderID = id }, commandType: CommandType.StoredProcedure);
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

        public async Task<Decimal> GetSalesByDate(DateTime initialDate, DateTime finalDate)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@InitialDate", initialDate);
            parameters.Add("@FinalDate", finalDate);

            return await db.QuerySingleOrDefaultAsync<Decimal>("GetSalesByDate", parameters, commandType: CommandType.StoredProcedure);; 
        }

        public async Task<IEnumerable<RecentOrderDTO>> GetOrdersByDate(DateTime initialDate, DateTime finalDate)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@InitialDate", initialDate);
            parameters.Add("@FinalDate", finalDate);

            return await db.QueryAsync<RecentOrderDTO>("GetOrdersByDate", parameters, commandType: CommandType.StoredProcedure);
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
            catch (MySqlException ex)
            {
                return new DbResponse<OrderDTO>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DbResponse<string>> CancelOrder(string id)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                await db.ExecuteAsync("CancelOrderTransaction", new { OrderID = id }, commandType: CommandType.StoredProcedure);

                return new DbResponse<string>
                {
                    Success = true
                };
            }
            catch (MySqlException ex)
            {
                return new DbResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
