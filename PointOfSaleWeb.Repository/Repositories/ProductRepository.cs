using Dapper;
using Dapper.Contrib.Extensions;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;
using System.Data;

namespace PointOfSaleWeb.Repository.Repositories;

public class ProductRepository(DbContext context) : IProductRepository
{
    private readonly DbContext _context = context;

    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        using IDbConnection db = _context.CreateConnection();

        return await db.QueryAsync<Product, Category, Product>(
            "sp_GetAllProducts",
            (product, category) =>
            {
                product.ProductCategory = category;
                return product;
            },
            splitOn: "CategoryID",
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts()
    {
        using IDbConnection db = _context.CreateConnection();

        return await db.QueryAsync<BestSellerProductDTO>(
            "sp_GetBestSellerProducts",
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<IEnumerable<ProductSoldByDateDTO>> GetProductsSoldByDate(DateTime initialDate, DateTime finalDate)
    {
        using IDbConnection db = _context.CreateConnection();

        return await db.QueryAsync<ProductSoldByDateDTO>(
            "GetProductsSoldByDate",
            new { InitialDate = initialDate, FinalDate = finalDate },
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<Product?> GetProductByID(string productId)
    {
        using IDbConnection db = _context.CreateConnection();

        return (await db.QueryAsync<Product, Category, Product>(
            "sp_GetProductById",
            (product, category) =>
            {
                product.ProductCategory = category;
                return product;
            },
            new { ProductID = productId },
            splitOn: "CategoryID",
            commandType: CommandType.StoredProcedure
        )).FirstOrDefault();
    }

    public async Task<Product?> AddNewProduct(Product product)
    {
        using IDbConnection db = _context.CreateConnection();

        var result = await db.QueryAsync<Product>(
            "sp_AddNewProduct",
            MapProductToParameters(product),
            commandType: CommandType.StoredProcedure
        );

        return result.FirstOrDefault();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        using IDbConnection db = _context.CreateConnection();

        var result = await db.QueryAsync<Product>(
            "sp_UpdateProduct",
            MapProductToParameters(product),
            commandType: CommandType.StoredProcedure
        );

        return result.FirstOrDefault();
    }

    public async Task<bool> DeleteProduct(string id)
    {
        using IDbConnection db = _context.CreateConnection();

        var product = new Product { ProductID = id };

        return await db.DeleteAsync(product);
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