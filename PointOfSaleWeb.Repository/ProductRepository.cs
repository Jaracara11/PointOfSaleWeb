using Dapper;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;
        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryAsync<Product>("GetAllProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<Product> GetProductByID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", id);

            return await db.QuerySingleOrDefaultAsync<Product>("GetProductByID", parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
