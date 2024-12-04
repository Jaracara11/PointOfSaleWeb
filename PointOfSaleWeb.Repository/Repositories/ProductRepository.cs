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

        public async Task<DbResponse<IEnumerable<Product>>> GetAllProducts()
        {
            return await ExecuteProductQuery("sp_GetAllProducts");
        }

        public async Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts()
        {
            //Pending refactor
            using IDbConnection db = _context.CreateConnection();

            return await db.QueryAsync<BestSellerProductDTO>(
                "sp_GetBestSellerProducts", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProductSoldByDateDTO>> GetProductsSoldByDate(DateTime initialDate, DateTime finalDate)
        {
            //Pending refactor
            using IDbConnection db = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@InitialDate", initialDate);
            parameters.Add("@FinalDate", finalDate);

            return await db.QueryAsync<ProductSoldByDateDTO>(
                "GetProductsSoldByDate", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetProductByID(string productId)
        {
            var response = await ExecuteProductQuery("sp_GetProductById", new { ProductID = productId });

            return response.Success ? response.Data?.FirstOrDefault() : null;
        }

        public async Task<DbResponse<Product>> AddNewProduct(Product product)
        {
            var response = await ExecuteProductQuery("sp_AddNewProduct", MapProductToParameters(product));

            return new DbResponse<Product>
            {
                Success = response.Success,
                Data = response.Data?.FirstOrDefault(),
                Message = response.Message
            };
        }

        public async Task<DbResponse<Product>> UpdateProduct(Product product)
        {
            var response = await ExecuteProductQuery("sp_UpdateProduct", MapProductToParameters(product));

            return new DbResponse<Product>
            {
                Success = response.Success,
                Data = response.Data?.FirstOrDefault(),
                Message = response.Message
            };
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

        private async Task<DbResponse<IEnumerable<Product>>> ExecuteProductQuery(
            string storedProcedureName, object? parameters = null)
        {
            using IDbConnection db = _context.CreateConnection();

            try
            {
                var result = await db.QueryAsync<Product, Category, Product>(
                    storedProcedureName,
                    (productData, categoryData) =>
                    {
                        productData.ProductCategory = categoryData;
                        return productData;
                    },
                    parameters,
                    splitOn: "CategoryID",
                    commandType: CommandType.StoredProcedure
                );

                return new DbResponse<IEnumerable<Product>>
                {
                    Success = true,
                    Data = result
                };
            }
            catch (SqlException ex)
            {
                return new DbResponse<IEnumerable<Product>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        private static DynamicParameters MapProductToParameters(Product product)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@ProductID", product.ProductID);
            parameters.Add("@ProductName", product.ProductName);
            parameters.Add("@ProductDescription", product.ProductDescription);
            parameters.Add("@ProductStock", product.ProductStock);
            parameters.Add("@ProductCost", product.ProductCost);
            parameters.Add("@ProductPrice", product.ProductPrice);
            parameters.Add("@ProductCategoryID", product.ProductCategory.CategoryID);

            return parameters;
        }
    }
}