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
        var validationResult = ValidationUtil.DateRangeValidation(initialDate, finalDate);

        return validationResult.Success
            ? Results.Ok(await _prodRepo.GetProductsSoldByDate(initialDate!.Value, finalDate!.Value))
            : ResponseUtil.CreateErrorResponse(
                "Invalid Date Range",
                validationResult.Message,
                StatusCodes.Status400BadRequest
            );
    }

    [HttpGet("{id}")]
    [ResponseCache(Duration = 5)]
    public async Task<IResult> GetProductByID(string id)
    {
        var product = await _prodRepo.GetProductByID(id);

        return product != null
            ? Results.Ok(product)
            : ResponseUtil.CreateNotFoundResponse(
                "Product Not Found",
                $"No product found with ID {id}. Please verify the product ID and try again."
            );
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> CreateProduct([FromBody] ProductInsertDTO product)
    {
        return await _prodRepo.AddNewProduct(product)
            ? Results.Created("/api/products", product)
            : ResponseUtil.CreateErrorResponse(
                "Failed to Add Product",
                "Product could not be added.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpPut("edit")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> UpdateProduct([FromBody] Product product)
    {
        return await _prodRepo.UpdateProduct(product)
            ? Results.Ok(product)
            : ResponseUtil.CreateErrorResponse(
                "Failed to Update Product",
                "Product could not be updated.",
                StatusCodes.Status400BadRequest
            );
    }

    [HttpDelete("{id}/delete")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IResult> DeleteProduct(string id)
    {
        return await _prodRepo.DeleteProduct(id)
            ? Results.NoContent()
            : ResponseUtil.CreateErrorResponse(
                "Failed to Delete Product",
                "Product not found or could not be deleted.",
                StatusCodes.Status400BadRequest
            );
    }
}