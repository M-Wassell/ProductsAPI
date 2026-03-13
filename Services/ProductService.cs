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
            new Product { Id = 1, Name = "Cup", Price = 3.99m, StockQuantity = 500, Category = "Crocery", Description = "Exotic tea cups made from Byson bones", IsActive = true },
            new Product { Id = 2, Name = "Toaster", Price = 4.99m, StockQuantity = 700, Category = "Crocery", Description = "Super shiny and quick toaster", IsActive = true },
            new Product { Id = 3, Name = "Bottle", Price = 3.99m, StockQuantity = 900, Category = "Sports", Description = "Very vague bottle", IsActive = true },
            new Product { Id = 4, Name = "TV", Price = 55.99m, StockQuantity = 300, Category = "Electronics", Description = "Big 55Inch TV", IsActive = true },
            new Product { Id = 5, Name = "Blanket", Price = 10.99m, StockQuantity = 100, Category = "Bedding", Description = "Soft blacnket made from unicorn fur", IsActive = true }
        };

        public Task<ServiceResponse<List<ProductDto>>> GetAll()
        {
            var response = new ServiceResponse<List<ProductDto>>();

            var activeProducts = Products
                .Where(p => p.IsActive)
                .ToList();

            if (!activeProducts.Any()) { 
                response.Success = false;
                response.Message = "Product List Could not be found";
                return Task.FromResult(response);
            }
            Console.WriteLine(Products.Count);
            Console.WriteLine(Products.Count(p => p.IsActive));

            response.Data = _mapper.Map<List<ProductDto>>(activeProducts);
            response.Success = true;
            response.Message = "Product List Found";
           
            return Task.FromResult(response);
        }

        public Task<ServiceResponse<ProductDto>> GetById(int id)
        {
            var response = new ServiceResponse<ProductDto>();

            if (id <= 0) { 
                response.Success = false;
                response.Message = "Product Id not found";
                return Task.FromResult(response);
            }

            var product = Products.FirstOrDefault(x => x.Id == id);

            var dto = _mapper.Map<ProductDto>(product); 

            response.Data = dto;
            response.Success = true;
            response.Message = "Product found";

            return Task.FromResult(response);
        }

        public Task<ServiceResponse<ProductDto>> Create(CreateProductDto newProduct)
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
            var product = _mapper.Map<Product>(newProduct);

            product.Id = productId;
            product.CreatedDate = DateTime.UtcNow;
            product.IsActive = true;
            
            Products.Add(product);

            response.Data = _mapper.Map<ProductDto>(product);
            response.Success = true;
            response.Message = $"Product has been Added";
            
            return Task.FromResult(response);
        }
        
        public Task<ServiceResponse<ProductDto>> Update(int id, UpdateProductDto upDatingProduct)
        {
            var response = new ServiceResponse<ProductDto>();

            if (id <= 0) { 
                response.Success = false;
                response.Message = "Existing Product does not exist";
                return Task.FromResult(response);
            }

            var product = Products.FirstOrDefault(prod => prod.Id == id);

            if(product == null){ 
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            if (String.IsNullOrWhiteSpace(upDatingProduct.Name)) {  
                response.Success = false;
                response.Message = "Product Name is required";
                return Task.FromResult(response);
            }

            _mapper.Map(upDatingProduct, product);

            product.UpdatedDate = DateTime.UtcNow;

            response.Data = _mapper.Map<ProductDto>(product);
            response.Success = true;
            response.Message = "Product Updated Successfully";
            
            return Task.FromResult(response);
        }

        public Task<ServiceResponse<bool>> Delete(int id)
        {
            var response = new ServiceResponse<bool>();
            if (id <= 0)
            {
                response.Success = false;
                response.Message = "Invalid Product Id";
                response.Data = false;
                return Task.FromResult(response);
            }

            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null)
            {
                response.Success = false;
                response.Message = "Product not found";
                return Task.FromResult(response);
            }

            existingProduct.IsActive = false;
            existingProduct.UpdatedDate = DateTime.UtcNow;
            

            response.Data = true;
            response.Success = true;
            response.Message = "Product Soft Delete Successfull";

            return Task.FromResult(response);
        }
    }
}
