using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using ProductsAPI.Dto;
using ProductsAPI.Dto.Query;
using ProductsAPI.Models;
using ProductsAPI.Services;
using ProductsAPI.Validators;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IValidator<PriceRangeQuery> _priceRangeQueryValidator;
        private readonly IValidator<CreateProductQuery> _createProductQueryValidator;


        public ProductsController(IProductService productService, IValidator<PriceRangeQuery> priceRangeQueryValidator, IValidator<CreateProductQuery> createProductQueryValidator)
        {
            _productService = productService;
            _priceRangeQueryValidator = priceRangeQueryValidator;
            _createProductQueryValidator = createProductQueryValidator;
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
        public async Task<ActionResult<ServiceResponse<ProductDto>>>Create(CreateProductQuery query) {

            var validationResult = await _createProductQueryValidator.ValidateAsync(query);
            if (!validationResult.IsValid ){ 
                return ValidationProblem(validationResult);
            }

            var result = await _productService.Create(query.Dto);

            if (!result.Success) {
                return BadRequest(result);
            }


            return CreatedAtAction(nameof(GetById), new { id = result.Data}, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(int id, UpdateProductDto dto)
        {
            if (id <= 0) {
                return NotFound();
            }
            var updated = await _productService.Update(id, dto);

            if (updated == null) { 
                return NotFound();
            }

            return Ok(updated);
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

        [HttpGet("by-price")]
        public async Task<ActionResult> GetExactPriceAsync([FromQuery] decimal price) {

            var result = await _productService.GetExactPriceAsync(price);

            if (!result.Success) {
                return NotFound(result.Message);
            }

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }

        [HttpGet("by-price-range")]
        public async Task<ActionResult> GetPriceRangeAsync([FromQuery] PriceRangeQuery query)
        {
            var validationResult = await _priceRangeQueryValidator.ValidateAsync(query);

            if (!validationResult.IsValid)
            {
                return ValidationProblem(validationResult);
            }

            var result = await _productService.GetPriceRangeAsync(query.MinPrice, query.MaxPrice);

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }

        [HttpGet("by-name")]
        public async Task<ActionResult> GetProductByNameAsync([FromQuery] string name)
        {
            var result = await _productService.GetProductByNameAsync(name);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }

        [HttpGet("by-category")]
        public async Task<ActionResult> GetProductByCategoryAsync([FromQuery] string category)
        {

            var result = await _productService.GetProductByCategoryAsync(category);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }

    }
}
