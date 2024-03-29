using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category?> GetCategoryByID(int id);
        Task<DbResponse<Category>> AddNewCategory(string categoryName);
        Task<DbResponse<Category>> UpdateCategory(Category category);
        Task<DbResponse<Category>> DeleteCategory(int id);
    }
}