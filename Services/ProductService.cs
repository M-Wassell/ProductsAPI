using ProductsAPI.Classess;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {

        private static readonly  List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, Name = "Cup", Price = 3.99m },
            new Product { Id = 2, Name = "Toaster", Price = 4.99m },
            new Product { Id = 3, Name = "Bottle", Price = 3.99m },
            new Product { Id = 4, Name = "TV", Price = 55.99m },
            new Product { Id = 5, Name = "Blanket", Price = 10.99m }
        };

        public Task<ServiceResponse<List<Product>>> GetAll()
        {
            var response = new ServiceResponse<List<Product>>();

            response.Data = Products;

            return Task.FromResult(response);
        }
        public Task<ServiceResponse<Product>> GetProductById(int id)
        {

            var response = new ServiceResponse<Product>();

            var product = Products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                response.Data = null;
            }
            else { 
                response.Data = product;
            }

                return Task.FromResult(response);

        }

        public Task<ServiceResponse<Product>> AddProduct(Product product)
        {
            var response = new ServiceResponse<Product>();

            if (product == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                response.Data = null;
            }

            if (String.IsNullOrWhiteSpace(product.Name)) {
                response.Success = false;
                response.Message = "Product Name is required";
                return Task.FromResult(response);
            }

            var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
            
            product.Id = newId;
            
            Products.Add(product);

            response.Data = product;

            return Task.FromResult(response);
            
        }
        
        public Task<ServiceResponse<Product>>? UpdateProduct(int id, Product upDatingProduct)
        {

            var response = new ServiceResponse<Product>();
            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if(existingProduct == null){ 
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            if (String.IsNullOrWhiteSpace(upDatingProduct.Name)) {  
                response.Success = false;
                response.Message = "Product Name is required";
                return Task.FromResult(response);
            }

            existingProduct.Name = upDatingProduct.Name;
            existingProduct.Price = upDatingProduct.Price;

            response.Data = existingProduct;

            return Task.FromResult(response);
        }

        public Task<ServiceResponse<Product>> DeleteProduct(int id)
        {
            var response = new ServiceResponse<Product>();
            if (id <= 0)
            {
                response.Success = false;
                response.Message = "Invalid Product Id";
                return Task.FromResult(response);
            }

            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null) { 
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            Products.Remove(existingProduct);
            response.Message = "Product Deleted Successfully";

            response.Data = existingProduct;

            return Task.FromResult(response);
        }

    }
}
