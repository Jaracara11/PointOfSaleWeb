using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.App.Utilities;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductRepository prodRepo) : ControllerBase
    {
        private readonly IProductRepository _prodRepo = prodRepo;

        [HttpGet]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() => 
            Ok((await _prodRepo.GetAllProducts()).Data);

        [HttpGet("best-sellers")]
        [ResponseCache(Duration = 300)]
        //public async Task<ActionResult<IEnumerable<BestSellerProductDTO>>> GetBestSellerProducts() => Ok(await _prodRepo.GetBestSellerProducts());
        public async Task<ActionResult<IEnumerable<BestSellerProductDTO>>> GetBestSellerProducts() => 
            Ok(new List<BestSellerProductDTO>());

        [HttpGet("sold-by-date")]
        [ResponseCache(Duration = 300)]
        public async Task<ActionResult<IEnumerable<ProductSoldByDateDTO>>> GetProductsSoldByDate(
            DateTime? initialDate, DateTime? finalDate)
        {
            //var dateValidationResult = ValidationUtil.DateRangeValidation(initialDate, finalDate);

            //return dateValidationResult.Success ? Ok(await _prodRepo.GetProductsSoldByDate(initialDate!.Value, finalDate!.Value))
            // : BadRequest(new { dateValidationResult.Message });
            return new List<ProductSoldByDateDTO>();
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 5)]
        public async Task<ActionResult<Product>> GetProductByID(string id)
        {
            var response = await _prodRepo.GetProductByID(id);

            return response != null ? Ok(response) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> AddNewProduct(Product product)
        {
            var response = await _prodRepo.AddNewProduct(product);

            return response.Success ? Created("Product", response.Data) : BadRequest(new { response.Message });
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            var response = await _prodRepo.UpdateProduct(product);

            return response.Success ? Ok(response) : BadRequest(new { response.Message });
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            bool isDeleted = await _prodRepo.DeleteProduct(id);

            return isDeleted ? NoContent() : BadRequest(new { Message = "Product not found or could not be deleted." });
        }
    }
}
