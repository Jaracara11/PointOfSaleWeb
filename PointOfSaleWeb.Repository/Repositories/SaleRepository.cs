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

        public async Task<IEnumerable<Discount>> GetDiscountsAvailable()
        {
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<Discount>("GetAllDiscounts", commandType: CommandType.StoredProcedure);
        }
    }
}
