using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PosDbContext _context;
        public CategoryRepository(PosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryAsync<Category>("GetAllCategories", commandType: CommandType.StoredProcedure);
        }

        public async Task<Category> GetCategoryByID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", id);

            return await db.QuerySingleOrDefaultAsync<Category>("GetCategoryByID", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<DbResponse<Category>> AddNewCategory(string categoryName)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryName", categoryName);

            try
            {
                await db.ExecuteAsync("AddNewCategory", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Category>
                {
                    Success = true
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<Category>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
