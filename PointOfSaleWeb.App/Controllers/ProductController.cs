using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController(IProductRepository prodRepo) : ControllerBase
{
    private readonly IProductRepository _prodRepo = prodRepo;

    [HttpGet]
    [ResponseCache(Duration = 5)]
    public async Task<IResult> GetAllProducts() =>
        Results.Ok(await _prodRepo.GetAllProducts());

    [HttpGet("best-sellers")]
    [ResponseCache(Duration = 300)]
    public async Task<IResult> GetBestSellerProducts() =>
        Results.Ok(await _prodRepo.GetBestSellerProducts());

    [HttpGet("sold-by-date")]
    [ResponseCache(Duration = 300)]
    public async Task<IResult> GetProductsSoldByDate([FromQuery] DateTime? initialDate, [FromQuery] DateTime? finalDate)
    {
        var dateValidationResult = ValidationUtil.DateRangeValidation(initialDate, finalDate);

        if (!dateValidationResult.Success)
        {
            return Results.BadRequest(new ProblemDetails
            {
                Title = "Invalid Date Range",
                Detail = dateValidationResult.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }

        var products = await _prodRepo.GetProductsSoldByDate(initialDate!.Value, finalDate!.Value);

        return Results.Ok(products);
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 5)]
    public async Task<IResult> GetProductByID(string id)
    {
        var product = await _prodRepo.GetProductByID(id);

        if (product == null)
        {
            return Results.NotFound(new ProblemDetails
            {
                Title = "Product Not Found",
                Detail = $"No product found with ID {id}. Please verify the product ID and try again.",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Results.Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> CreateProduct([FromBody] ProductInsertDTO product)
    {
        var success = await _prodRepo.AddNewProduct(product);

        return success
            ? Results.Created("/api/products", product)
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Failed to Add Product",
                Detail = "Product could not be added.",
                Status = StatusCodes.Status400BadRequest
            });
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> UpdateProduct([FromBody] Product product)
    {
        var updatedProduct = await _prodRepo.UpdateProduct(product);

        return updatedProduct
            ? Results.Ok(product)
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Failed to Update Product",
                Detail = "Product could not be updated.",
                Status = StatusCodes.Status400BadRequest
            });
    }

    [HttpDelete("{id}/delete")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> DeleteProduct(string id)
    {
        var isDeleted = await _prodRepo.DeleteProduct(id);

        return isDeleted
            ? Results.NoContent()
            : Results.BadRequest(new ProblemDetails
            {
                Title = "Failed to Delete Product",
                Detail = "Product not found or could not be deleted.",
                Status = StatusCodes.Status400BadRequest
            });
    }
}