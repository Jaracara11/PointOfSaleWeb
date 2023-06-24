using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;
using System.Text.Json;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class SaleRepository: ISaleRepository
    {
        private readonly DbContext _context;

        public SaleRepository(DbContext context)
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

        public async Task<DbResponse<OrderDTO>> NewOrderTransaction(Order order)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            //parameters.Add("@OrderID", Guid.NewGuid().ToString());
            parameters.Add("@User", order.User);
            parameters.Add("@Products", JsonSerializer.Serialize(order.Products));
            parameters.Add("@Discount", order.Discount);
            parameters.Add("@OrderTotal", order.OrderTotal);
            parameters.Add("@OrderDate", order.OrderDate);

            try
            {
                var orderResponse = await db.QuerySingleOrDefaultAsync<OrderDTO>("NewOrderTransaction", parameters, commandType: CommandType.StoredProcedure);

                orderResponse.Products = order.Products;

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
