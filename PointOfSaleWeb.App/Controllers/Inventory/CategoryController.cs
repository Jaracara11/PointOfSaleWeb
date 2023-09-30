using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Inventory
{
    [Route("api/categories")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories() => Ok(await _catRepo.GetAllCategories());

        [HttpGet("{id}")]
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

            return response.Success ? Created("Category", response.Data) : BadRequest(new { error = response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateCategory(Category category)
        {
            var response = await _catRepo.UpdateCategory(category);

            return response.Success ? Ok(response) : BadRequest(new { error = response.Message });
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _catRepo.DeleteCategory(id);

            return response.Success ? NoContent() : BadRequest(new { error = response.Message });
        }
    }
}
