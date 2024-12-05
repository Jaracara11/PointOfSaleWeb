using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers;

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
            : ResponseUtil.CreateNotFoundResponse(
                "Category Not Found",
                $"No category found with ID {id}."
            );
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> AddNewCategory([FromBody] string categoryName)
    {
        var success = await _catRepo.AddNewCategory(categoryName);

        return success
            ? Results.Created("/api/categories", categoryName)
            : ResponseUtil.CreateErrorResponse(
                "Failed to Add Category",
                "Category could not be added.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> UpdateCategory([FromBody] Category category)
    {
        var updatedCategory = await _catRepo.UpdateCategory(category);

        return updatedCategory != null
            ? Results.Ok(updatedCategory)
            : ResponseUtil.CreateErrorResponse(
                "Failed to Update Category",
                "Category could not be updated.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpDelete("{id}/delete")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> DeleteCategory(int id)
    {
        return await _catRepo.DeleteCategory(id)
            ? Results.NoContent()
            : ResponseUtil.CreateNotFoundResponse(
                "Category Not Found",
                $"No category found with ID {id}."
            );
    }
}