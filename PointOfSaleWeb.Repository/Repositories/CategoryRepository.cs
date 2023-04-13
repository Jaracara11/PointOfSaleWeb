using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbContext _context;
        public CategoryRepository(DbContext context)
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

            return await db.QuerySingleOrDefaultAsync<Category>("GetCategoryById", parameters, commandType: CommandType.StoredProcedure);
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

            try
            {
                category = await db.QuerySingleOrDefaultAsync<Category>("UpdateCategory", parameters, commandType: CommandType.StoredProcedure);

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
