using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace Inventory.API.Controllers.Inventory
{
    [Route("api/category")]
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
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _catRepo.GetAllCategories();

            if (categories == null || !categories.Any())
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<Category>> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> AddNewCategory(Category category)
        {
            var response = await _catRepo.AddNewCategory(category.CategoryName);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("Category", response.Data);
        }

        [HttpPut("{id}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCategory(int id, Category category)
        {
            category.CategoryID = id;

            var response = await _catRepo.UpdateCategory(category);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _catRepo.DeleteCategory(id);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
