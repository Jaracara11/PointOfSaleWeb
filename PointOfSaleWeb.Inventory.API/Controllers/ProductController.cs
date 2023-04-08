using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _prodRepo;
        public ProductController(IProductRepository prodRepo)
        {
            _prodRepo = prodRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _prodRepo.GetAllProducts();

            if (products == null || !products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

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

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            product.ProductID = id;

            var response = await _prodRepo.UpdateProduct(product);

            if (!response.Success)
            {
                ModelState.AddModelError("ProductError", response.Message);
                return BadRequest(ModelState);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var response = await _prodRepo.DeleteProduct(id);

            if (!response.Success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
