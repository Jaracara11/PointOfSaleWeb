using Dapper;
using Microsoft.Data.SqlClient;
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

        public async Task<IEnumerable<Product>> GetProductsByCategoryID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductCategoryID", id);

            return await db.QueryAsync<Product>("GetProductsByCategoryId", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Product> GetProductByID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", id);

            return await db.QuerySingleOrDefaultAsync<Product>("GetProductByID", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<DbResponse<Product>> AddNewProduct(Product product)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@ProductDescription", product.ProductDescription);
            parameters.Add("@ProductStock", product.ProductStock);
            parameters.Add("@ProductCost", product.ProductCost);
            parameters.Add("@ProductPrice", product.ProductPrice);
            parameters.Add("@ProductCategoryID", product.ProductCategoryID);

            try
            {
                product = await db.QuerySingleOrDefaultAsync<Product>("AddNewProduct", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Product>
                {
                    Success = true,
                    Data = product
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<Product>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<DbResponse<Product>> UpdateProduct(Product product)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", product.ProductID);
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@ProductDescription", product.ProductDescription);
            parameters.Add("@ProductPrice", product.ProductPrice);
            parameters.Add("@ProductCost", product.ProductCost);
            parameters.Add("@ProductStock", product.ProductStock);
            parameters.Add("@ProductCategoryID", product.ProductCategoryID);

            try
            {
                product = await db.QuerySingleOrDefaultAsync<Product>("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Product>
                {
                    Success = true,
                    Message = "Product updated!",
                    Data = product
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<Product>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<DbResponse<Product>> DeleteProduct(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", id);

            try
            {
                await db.ExecuteAsync("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Product>
                {
                    Success = true
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<Product>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
