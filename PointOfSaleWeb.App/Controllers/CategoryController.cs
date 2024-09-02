using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController(ICategoryRepository catRepo) : ControllerBase
    {
        private readonly ICategoryRepository _catRepo = catRepo;

        [HttpGet]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories() => Ok(await _catRepo.GetAllCategories());

        [HttpGet("{id}")]
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<Category>> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            return category != null ? Ok(category) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Category>> AddNewCategory(Category category)
        {
            var response = await _catRepo.AddNewCategory(category.CategoryName);

            return response.Success ? Created("Category", response.Data) : BadRequest(new { response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateCategory(Category category)
        {
            var response = await _catRepo.UpdateCategory(category);

            return response.Success ? Ok(response) : BadRequest(new { response.Message });
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _catRepo.DeleteCategory(id);

            return response.Success ? NoContent() : BadRequest(new { response.Message });
        }
    }
}
