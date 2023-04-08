using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryDbContext _context;
        public CategoryRepository(InventoryDbContext context)
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
                var category = await db.QuerySingleOrDefaultAsync<Category>("AddNewCategory", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Category>
                {
                    Success = true,
                    Data = category
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

        public async Task<DbResponse<Category>> UpdateCategory(Category category)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", category.CategoryID);
            parameters.Add("@CategoryName", category.CategoryName);
            parameters.Add("@UpdatedCategoryName", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

            try
            {
                await db.ExecuteAsync("UpdateCategory", parameters, commandType: CommandType.StoredProcedure);

                category.CategoryName = parameters.Get<string>("@UpdatedCategoryName");

                return new DbResponse<Category>
                {
                    Success = true,
                    Message = "Category updated!",
                    Data = category
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


        public async Task<DbResponse<Category>> DeleteCategory(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryID", id);

            try
            {
                await db.ExecuteAsync("DeleteCategory", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Category>
                {
                    Success = true
                };
            }
            catch (SqlException)
            {
                return new DbResponse<Category>
                {
                    Success = false
                };
            }
        }
    }
}
