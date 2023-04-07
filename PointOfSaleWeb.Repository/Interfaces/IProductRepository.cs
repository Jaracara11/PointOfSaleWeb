﻿using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetProductsByCategory(int id);
        Task<Product> GetProductByID(int id);
        Task<DbResponse<Product>> AddNewProduct(Product product);
    }
}