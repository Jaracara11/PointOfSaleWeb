using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _catRepo;
        public CategoryController(ICategoryRepository catRepo)
        {
            _catRepo = catRepo;
        }

        [HttpGet]
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
        public async Task<ActionResult<Category>> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category != null)
            {
                return Ok(category);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, Category category)
        {
            category.CategoryID = id;

            var response = await _catRepo.UpdateCategory(category);

            if (!response.Success)
            {
                ModelState.AddModelError("CategoryError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var response = await _catRepo.DeleteCategory(id);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
