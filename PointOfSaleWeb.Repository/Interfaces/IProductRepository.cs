using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<BestSellerProductDTO>> GetBestSellerProducts();
        Task<IEnumerable<ProductSoldByDateDTO>> GetProductsSoldByDate(DateTime initialDate, DateTime finalDate);
        Task<Product?> GetProductByID(string productId);
        Task<bool> AddNewProduct(ProductInsertDTO productData);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);
    }
}