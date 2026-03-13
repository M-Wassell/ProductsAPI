using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using ProductsAPI.Dto;
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
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetAll()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            if (id <= 0) {
                return NotFound();
            }

            var product = _productService.GetById(id);

            if (product == null){ 
                return NotFound();
            }
                
            return Ok(await product);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ProductDto>>>Create(CreateProductDto dto) {

            if (dto == null){ 
                return NotFound();
            }

            var result = await _productService.Create(dto);

            if (!result.Success) {
                return BadRequest(result);
            }
            return Ok(result);


            //return CreatedAtAction(nameof(GetProductById), new { id = createProduct.Id}, await createProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductDto dto)
        {
            if (id <= 0) {
                return NotFound();
            }
            var updated = _productService.Update(id, dto);

            if (updated == null) { 
                return NotFound();
            }

            return Ok(await updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id) {

            var result = await _productService.Delete(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return NoContent();
        }

    }
}
