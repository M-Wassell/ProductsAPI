using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using Contracts.Dto;
using ProductsAPI.Dto.Query;
using ProductsAPI.Models;
using ProductsAPI.Services;
using ProductsAPI.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Threading.Tasks;
using WebAPI_Project.ErrorHandling;
using static ProductsAPI.Enums.Status;
//namespace Product.Contracts.Dto
namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IValidator<PriceRangeQuery> _priceRangeQueryValidator;
        private readonly IValidator<CreateQuery> _createProductQueryValidator;
        private readonly IValidator<UpdateProductQuery> _updateProductQueryValidator;


        public ProductsController(IProductService productService, IValidator<PriceRangeQuery> priceRangeQueryValidator, 
            IValidator<CreateQuery> createProductQueryValidator, IValidator<UpdateProductQuery> updateProductQuery)
        {
            _productService = productService;
            _priceRangeQueryValidator = priceRangeQueryValidator;
            _createProductQueryValidator = createProductQueryValidator;
            _updateProductQueryValidator = updateProductQuery;
        }

        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetAll(pageNumber, pageSize);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductId(int id) {

            var result = await _productService.GetProductById(id);

            return result.Status switch 
            {
                ServiceStatus.NotFound => NotFound(),
                ServiceStatus.Deleted => Ok(result),
                ServiceStatus.Success => Ok(result),
                _ => Problem(result.Message)
            };
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateQuery query, [FromServices] IValidator<CreateQuery> validator) {

            await validator.ValidateAndThrowAsync(query);

            var result = await _productService.Create(query.CreateDto);

            return Ok(result);

            
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> Update([FromRoute]int id, [FromBody] UpdateProductQuery request, [FromServices] IValidator<UpdateProductQuery> validator)
        {
            request.Id = id;
            await validator.ValidateAndThrowAsync(request);
            
            var updated = await _productService.Update(request.Id, request.Dto);

            

            return updated.Success ? Ok(updated.Data) : Problem(updated.Message);
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
        public async Task<ActionResult> GetPriceRange([FromQuery] PriceRangeQuery query, [FromServices] IValidator<PriceRangeQuery> validator, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _productService.GetPriceRangeAsync(query.MinPrice, query.MaxPrice, pageNumber, pageSize);
    
            await validator.ValidateAndThrowAsync(query);

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
        public async Task<ActionResult> GetProductByCategoryAsync([FromQuery] string category, int pageNumber = 1, int pageSize = 10)
        {

            var result = await _productService.GetProductByCategoryAsync(category, pageNumber, pageSize);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }

        [HttpGet("by-date-created")]
        public async Task<ActionResult> GetProductsByCreatedDate([FromQuery] string createdDate, int pageNumber = 1, int pageSize = 10)
        {

            var result = await _productService.GetProductsByCreatedDate(createdDate, pageNumber, pageSize);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return result.Success ? Ok(result.Data) : Problem(result.Message);
        }


    }
}
