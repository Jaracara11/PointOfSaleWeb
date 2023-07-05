using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Models.DTOs;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Inventory
{
    [Route("api/product")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _prodRepo;
        public ProductController(IProductRepository prodRepo)
        {
            _prodRepo = prodRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts() => Ok(await _prodRepo.GetAllProducts());

        [HttpGet("best-sellers")]
        [ResponseCache(Duration = 43200)]
        public async Task<ActionResult<IEnumerable<BestSellerProductDTO>>> GetBestSellerProducts() => Ok(await _prodRepo.GetBestSellerProducts());

        [HttpGet("category/{id}")]
        public async Task<ActionResult<Product>> GetProductsByCategoryID(int id)
        {
            var products = await _prodRepo.GetProductsByCategoryID(id);

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByID(int id)
        {
            var product = await _prodRepo.GetProductByID(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Product>> AddNewProduct(Product product)
        {
            var response = await _prodRepo.AddNewProduct(product);

            if (!response.Success)
            {
                ModelState.AddModelError("ProductError", response.Message);
                return BadRequest(ModelState);
            }

            return Created("Product", response.Data);
        }

        [HttpPut("edit")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            var response = await _prodRepo.UpdateProduct(product);

            if (!response.Success)
            {
                ModelState.AddModelError("ProductError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}/delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var response = await _prodRepo.DeleteProduct(id);

            if (!response.Success)
            {
                ModelState.AddModelError("ProductError", response.Message);
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
