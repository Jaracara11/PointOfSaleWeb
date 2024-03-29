using Dapper;
using Microsoft.Data.SqlClient;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _context;
        public ProductRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryAsync<Product>("GetAllProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts()
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryAsync<BestSellerProductDTO>("GetBestSellerProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProductSoldByDateDTO>> GetProductsSoldByDate(DateTime initialDate, DateTime finalDate)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@InitialDate", initialDate);
            parameters.Add("@FinalDate", finalDate);

            return await db.QueryAsync<ProductSoldByDateDTO>("GetProductsSoldByDate", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryID(int id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QueryAsync<Product>("GetProductsByCategoryId", new { ProductCategoryID = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetProductByID(string id)
        {
            using IDbConnection db = _context.CreateConnection();
            return await db.QuerySingleOrDefaultAsync<Product>("GetProductById", new { ProductID = id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<DbResponse<Product>> AddNewProduct(Product product)
        {
            using IDbConnection db = _context.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@ProductID", product.ProductID);
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@ProductDescription", product.ProductDescription);
            parameters.Add("@ProductStock", product.ProductStock);
            parameters.Add("@ProductCost", product.ProductCost);
            parameters.Add("@ProductPrice", product.ProductPrice);
            parameters.Add("@ProductCategoryID", product.ProductCategoryID);

            Product? newProduct;

            try
            {
                newProduct = await db.QuerySingleOrDefaultAsync<Product>("AddNewProduct", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Product>
                {
                    Success = true,
                    Data = newProduct
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

            Product? updatedProduct;

            try
            {
                updatedProduct = await db.QuerySingleOrDefaultAsync<Product>("UpdateProduct", parameters, commandType: CommandType.StoredProcedure);

                return new DbResponse<Product>
                {
                    Success = true,
                    Message = "Product updated!",
                    Data = updatedProduct
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


        public async Task<DbResponse<Product>> DeleteProduct(string id)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                await db.ExecuteAsync("DeleteProduct", new { ProductID = id }, commandType: CommandType.StoredProcedure);

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
