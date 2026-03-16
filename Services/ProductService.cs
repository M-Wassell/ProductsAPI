using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using ProductsAPI.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private static List<Product> _products = new();
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, ApplicationDbContext context, ILogger<ProductService> logger) { 
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        //private static readonly  List<Product> Products = new List<Product>()
        //{
        //    new Product { Id = 1, Name = "Cup", Price = 3.99m, StockQuantity = 500, Category = "Crocery", Description = "Exotic tea cups made from Byson bones", IsActive = true },
        //    new Product { Id = 2, Name = "Toaster", Price = 4.99m, StockQuantity = 700, Category = "Crocery", Description = "Super shiny and quick toaster", IsActive = true },
        //    new Product { Id = 3, Name = "Bottle", Price = 3.99m, StockQuantity = 900, Category = "Sports", Description = "Very vague bottle", IsActive = true },
        //    new Product { Id = 4, Name = "TV", Price = 55.99m, StockQuantity = 300, Category = "Electronics", Description = "Big 55Inch TV", IsActive = true },
        //    new Product { Id = 5, Name = "Blanket", Price = 10.99m, StockQuantity = 100, Category = "Bedding", Description = "Soft blacnket made from unicorn fur", IsActive = true }
        //};



        public async Task<ServiceResponse<List<ProductDto>>> GetAll()
        {
            _logger.LogInformation("Attempting to fetch all products");

            var response = new ServiceResponse<List<ProductDto>>();
            var products = await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();
            try
            {

                if (products.Count <= 0) {
                    _logger.LogError("Failed to fetch all products");
                    response.Success = false;
                    response.Message = "Product List Could not be found";
                    return response;
                }

                _logger.LogInformation("Success");
                response.Data = _mapper.Map<List<ProductDto>>(products);
                response.Success = true;
                response.Message = "Product List Found";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }
           
            return response;
        }

        public async Task<ServiceResponse<ProductDto>> GetById(int id)
        {
            _logger.LogInformation("Attempting to fetch product by {id}", id);
            var response = new ServiceResponse<ProductDto>();

            try
            {
                if (id <= 0) {
                    _logger.LogError("Failed to fetch product {id}", id);
                    response.Success = false;
                    response.Message = "Product Id not found";
                    return response;
                }

                var product = await _context.Products
                    .Where(p => p.IsActive)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var dto = _mapper.Map<ProductDto>(product); 
                await _context.SaveChangesAsync();

                response.Data = dto;
                response.Success = true;
                response.Message = "Product found";

                _logger.LogInformation("Successfully fetched product by {id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductDto>> Create(CreateProductDto createProductDto)
        {
            _logger.LogInformation("Attempting to create new Product");
            var response = new ServiceResponse<ProductDto>();

            try
            {
                if (String.IsNullOrWhiteSpace(createProductDto.Name)) {
                    _logger.LogError("Failed to fetch product {newProduct}", createProductDto);
                    response.Success = false;
                    response.Message = "Product Name is required";
                    return response;
                }

                var product = _mapper.Map<Product>(createProductDto);
                product.CreatedDate = DateTime.UtcNow;
                product.IsActive = true;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<ProductDto>(product);
                response.Success = true;
                response.Message = $"Product has been Added";
                _logger.LogInformation("Created product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }

            return response;
        }
        
        public async Task<ServiceResponse<ProductDto>> Update(int id, UpdateProductDto updateProductDto)
        {
            _logger.LogInformation("Attempting to update Product {id}", id);
            var response = new ServiceResponse<ProductDto>();
            var product = await _context.Products.FirstOrDefaultAsync(prod => prod.Id == id);

            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Failed to fetch product {id}", id);
                    response.Success = false;
                    response.Message = "Product Id not found";
                    return response;
                }

                _mapper.Map(updateProductDto, product);

                product.UpdatedDate = DateTime.UtcNow;

                response.Data = _mapper.Map<ProductDto>(product);
                response.Success = true;
                response.Message = "Product Updated Successfully";
                _logger.LogInformation("Updated product successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> Delete(int id)
        {
            _logger.LogInformation("Attempting to delete product {id}", id);
            var response = new ServiceResponse<bool>();

            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Failed to fetch product {id}", id);
                    response.Success = false;
                    response.Message = "Invalid Product Id";
                    response.Data = false;
                    return response;
                }

                var product = await _context.Products.FirstOrDefaultAsync(prod => prod.Id == id);

                if (product == null)
                {
                    _logger.LogWarning("Failed to deleted product {id}", id);
                    response.Success = false;
                    response.Message = "Product not found";
                    return response;
                }

                product.IsActive = false;
                product.UpdatedDate = DateTime.UtcNow;


                response.Data = true;
                response.Success = true;
                response.Message = "Product Soft Delete Successfull";
                _logger.LogInformation("Soft Delete Successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }
            
            return response;
        }
    }
}
