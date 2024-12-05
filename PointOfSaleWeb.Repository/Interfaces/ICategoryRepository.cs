using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryByID(int id);
        Task<bool> AddNewCategory(string categoryName);
        Task<Category> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}