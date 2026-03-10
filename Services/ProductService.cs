using AutoMapper;
using ProductsAPI.Classess;
using ProductsAPI.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
        
    {
        private readonly IMapper _mapper;
        public ProductService(IMapper mapper) { 
            _mapper = mapper;
        }

        private static readonly  List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, Name = "Cup", Price = 3.99m },
            new Product { Id = 2, Name = "Toaster", Price = 4.99m },
            new Product { Id = 3, Name = "Bottle", Price = 3.99m },
            new Product { Id = 4, Name = "TV", Price = 55.99m },
            new Product { Id = 5, Name = "Blanket", Price = 10.99m }
        };

        public Task<ServiceResponse<List<ProductDto>>> GetAll()
        {
            var response = new ServiceResponse<List<ProductDto>>();

            if (Products.Count <= 0) { 
                response.Success = false;
                response.Message = "Product List Could not be found";
                return Task.FromResult(response);
            }


            response.Data = Products
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList();

            response.Success = true;
            response.Message = "Product List Found";
           
            return Task.FromResult(response);
        }

        public Task<ServiceResponse<ProductDto>> GetProductById(int id)
        {
            var response = new ServiceResponse<ProductDto>();

            if (id <= 0) { 
                response.Success = false;
                response.Message = "Product Id not found";
                return Task.FromResult(response);
            }

            var product = Products.FirstOrDefault(x => x.Id == id);

            // Manual Mapping
            //var dto = new ProductDto{ 
            //    Name = product.Name,
            //    Price = product.Price
            //};

            var dto = _mapper.Map<ProductDto>(product); 

            response.Data = dto;
            response.Success = true;
            response.Message = "Product found";

            return Task.FromResult(response);
        }

        public Task<ServiceResponse<ProductDto>> AddProduct(ProductDto newProduct)
        {
            var response = new ServiceResponse<ProductDto>();

            if (newProduct == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            if (String.IsNullOrWhiteSpace(newProduct.Name)) {
                response.Success = false;
                response.Message = "Product Name is required";
                return Task.FromResult(response);
            }

            var productId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;

            var product = new Product
            {
                Name = newProduct.Name,
                Price = newProduct.Price
            };
            
            product.Id = productId;
            Products.Add(product);

            var dto = new ProductDto {
                Name = newProduct.Name,
                Price = newProduct.Price
            };

            response.Data = dto;
            response.Success = true;
            response.Message = $"Product Has been Added";
            
            return Task.FromResult(response);
        }
        
        public Task<ServiceResponse<ProductDto>>? UpdateProduct(int id, ProductDto upDatingProduct)
        {
            var response = new ServiceResponse<ProductDto>();

            if (id <= 0) { 
                response.Success = false;
                response.Message = "Existing Product does not exist";
                return Task.FromResult(response);
            }

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
            
            var dto = new ProductDto{ 
                Name = upDatingProduct.Name,
                Price = upDatingProduct.Price,
            };
            
            //Manually mapped
            response.Data = dto;
            response.Success = true;
            response.Message = "Product Updated Successfully";
            
            return Task.FromResult(response);
        }

        public Task<ServiceResponse<ProductDto>> DeleteProduct(int id)
        {
            var response = new ServiceResponse<ProductDto>();
            if (id <= 0)
            {
                response.Success = false;
                response.Message = "Invalid Product Id";
                return Task.FromResult(response);
            }

            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            Products.Remove(existingProduct);

            var dto = new ProductDto { 
                Name = existingProduct.Name,
                Price = existingProduct.Price,
            };

            response.Data = dto;
            response.Success = true;
            response.Message = "Product Deleted Successfully";

            return Task.FromResult(response);
        }
    }
}
