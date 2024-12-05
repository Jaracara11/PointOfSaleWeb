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
        public async Task<IResult> GetAllCategories() =>
            Results.Ok(await _catRepo.GetAllCategories());

        [HttpGet("{id}")]
        [ResponseCache(Duration = 300)]
        public async Task<IResult> GetCategoryByID(int id)
        {
            var category = await _catRepo.GetCategoryByID(id);

            return category != null
                ? Results.Ok(category)
                : Results.NotFound(new ProblemDetails
                {
                    Title = "Category Not Found",
                    Detail = $"No category found with ID {id}.",
                    Status = StatusCodes.Status404NotFound
                });
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IResult> AddNewCategory([FromBody] string categoryName)
        {
            var success = await _catRepo.AddNewCategory(categoryName);

            return success
                ? Results.Created("/api/categories", categoryName)
                : Results.BadRequest(new ProblemDetails
                {
                    Title = "Failed to Add Category",
                    Detail = "Category could not be added.",
                    Status = StatusCodes.Status400BadRequest
                });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IResult> UpdateCategory([FromBody] Category category)
        {
            var updatedCategory = await _catRepo.UpdateCategory(category);

            return updatedCategory != null
                ? Results.Ok(updatedCategory)
                : Results.BadRequest(new ProblemDetails
                {
                    Title = "Failed to Update Category",
                    Detail = "Category could not be updated.",
                    Status = StatusCodes.Status400BadRequest
                });
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IResult> DeleteCategory(int id)
        {
            return await _catRepo.DeleteCategory(id)
                ? Results.NoContent()
                : Results.NotFound(new ProblemDetails
                {
                    Title = "Category Not Found",
                    Detail = $"No category found with ID {id}.",
                    Status = StatusCodes.Status404NotFound
                });
        }
    }
}