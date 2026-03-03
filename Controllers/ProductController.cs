using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using ProductsAPI.Services;
using System.Collections.Generic;
using System.Data.Common;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        //private static readonly List<Product> Products = new List<Product>()
        //{
        //    new Product { Id = 1, Name = "Cup", Price = 3.99m },
        //    new Product { Id = 2, Name = "Toaster", Price = 4.99m },
        //    new Product { Id = 3, Name = "Bottle", Price = 3.99m },
        //    new Product { Id = 4, Name = "TV", Price = 55.99m },
        //    new Product { Id = 5, Name = "Blanket", Price = 10.99m }
        //};

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(_productService.GetAll());
        }


        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null){ return NotFound();}
                
            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product product) {

            var createProduct = _productService.AddProduct(product);

            return CreatedAtAction(nameof(GetProductById), new { id = createProduct.Id}, createProduct);
        }

        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, Product upDatingProduct)
        {
            var updated = _productService.UpdateProduct(id, upDatingProduct);

            if (updated == null) { 
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id) {

            var deleted = _productService.DeleteProduct(id);

            if (deleted == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /*
         * Route("Function call") allows us to pass in the function of another Action Result
         * Leaving us with this Route https://localhost:7239/api/Products/GetAll
         * This is not good naming but woth noting.
         */

        //[Route("GetAll")]
        //[HttpGet]
        //public ActionResult<IEnumerable<Product>> Get()
        //{
        //    return Ok(Products);
        //}

    }
}
