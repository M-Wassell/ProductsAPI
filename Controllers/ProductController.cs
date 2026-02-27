using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Classess;
using System.Collections.Generic;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, Name = "Cup", Price = 3.99m },
            new Product { Id = 2, Name = "Toaster", Price = 4.99m },
            new Product { Id = 3, Name = "Salt Shaker", Price = 0.99m }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(Products);
        }


        [HttpGet("{id}")]
        public ActionResult<Product> GetProductById(int id)
        {

            var product = Products.FirstOrDefault(prod => prod.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else {
                return Ok(product);
            }
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product product) {

            var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            product.Id = newId;

            Products.Add(product);


            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                product
            );
        }

        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, Product upDatingProduct)
        {
            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = upDatingProduct.Name;
            existingProduct.Price = upDatingProduct.Price;


                return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id) {

            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null) {
                return NotFound();
            }

            Products.Remove(existingProduct);

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
