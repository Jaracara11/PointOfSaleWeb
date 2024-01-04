using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Inventory
{
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductRepository prodRepo) : ControllerBase
    {
        private readonly IProductRepository _prodRepo = prodRepo;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() => Ok(await _prodRepo.GetAllProducts());

        [HttpGet("best-sellers")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<BestSellerProductDTO>>> GetBestSellerProducts() => Ok(await _prodRepo.GetBestSellerProducts());

        [HttpGet("sold-by-date")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<ProductSoldByDateDTO>>> GetProductsSoldByDate(DateTime? initialDate, DateTime? finalDate)
        {
            var dateValidationResult = ValidationHelper.DateRangeValidation(initialDate, finalDate);

            if (!dateValidationResult.Success)
            {
                return BadRequest(new { error = dateValidationResult.Message });
            }

            var products = await _prodRepo.GetProductsSoldByDate(initialDate!.Value, finalDate!.Value);

            return products != null && products.Any() ? Ok(products) : NotFound();
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryID(int id) => Ok(await _prodRepo.GetProductsByCategoryID(id));

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByID(string id)
        {
            var product = await _prodRepo.GetProductByID(id);

            return product != null ? Ok(product) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> AddNewProduct(Product product)
        {
            var response = await _prodRepo.AddNewProduct(product);

            return response.Success ? Created("Product", response.Data) : BadRequest(new { error = response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            var response = await _prodRepo.UpdateProduct(product);

            return response.Success ? Ok(response) : BadRequest(new { error = response.Message });
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            var response = await _prodRepo.DeleteProduct(id);

            return response.Success ? NoContent() : BadRequest(new { error = response.Message });
        }
    }
}
