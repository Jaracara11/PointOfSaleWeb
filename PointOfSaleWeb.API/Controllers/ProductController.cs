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

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetProductByID(int id)
        {
            var product = await _prodRepo.GetProductByID(id);

            if (product == null)
            {
                return NotFound();
            }

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
