using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using ProductsAPI.Models;
using ProductsAPI.Services;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAll()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            if (id <= 0) {
                return NotFound();
            }

            var product = _productService.GetProductById(id);

            if (product == null){ 
                return NotFound();
            }
                
            return Ok(await product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(Product product) {

            if (product == null){ 
                return NotFound();
            }

            var createProduct = _productService.AddProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { id = createProduct.Id}, await createProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product upDatingProduct)
        {
            if (id <= 0) {
                return NotFound();
            }
            var updated = _productService.UpdateProduct(id, upDatingProduct);

            if (updated == null) { 
                return NotFound();
            }

            return Ok(await updated);
        }

        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id) {

            if (id <= 0) {
                return NotFound();
            }

            var deleted = _productService.DeleteProduct(id);

            if (deleted == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
