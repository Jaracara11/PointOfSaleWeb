using Dapper;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

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

        public async Task<IEnumerable<Discount>> GetDiscountsByUsername(string username)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            return await db.QueryAsync<Discount>("GetDiscountsByUsername", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
