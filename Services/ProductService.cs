using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using ProductsAPI.Dto;
using ProductsAPI.Models;
using ProductsAPI.Repository;
using System.Xml.Linq;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductService> _logger;
        private static List<Product> _products = new();


        private readonly IMapper _mapper;
        public ProductService(IMapper mapper, ILogger<ProductService> logger, IProductRepository repo) { 
            _mapper = mapper;
            _logger = logger;
            _repo = repo;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetAll()
        {
            _logger.LogInformation("Attempting to fetch all products");

            var response = new ServiceResponse<List<ProductDto>>();
            var products = await _repo.GetAllAsync();
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

                var product = await _repo.GetByIdAsync(id);

                var dto = _mapper.Map<ProductDto>(product); 
                await _repo.SaveChangesAsync();

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

                await _repo.CreateAsync(product);
                await _repo.SaveChangesAsync();

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
            var product = await _repo.GetByIdAsync(id);

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

                var product = await _repo.GetByIdAsync(id);

                if (product == null)
                {
                    _logger.LogWarning("Failed to deleted product {id}", id);
                    response.Success = false;
                    response.Message = "Product not found";
                    return response;
                }

                product.IsActive = false;
                product.UpdatedDate = DateTime.UtcNow;

                await _repo.SaveChangesAsync();

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

        public async Task<ServiceResponse<List<ProductDto>>> GetExactPriceAsync(decimal price)
        {
            _logger.LogInformation("Attempting to fetch product by price");
            var response = new ServiceResponse<List<ProductDto>>();

            try
            {
                if (price < 0) {
                    _logger.LogWarning("Price not found");
                    response.Success = false;
                    response.Message = "Price not found";
                    return response;
                }

                var product = await _repo.GetExactPriceAsync(price);

                if (product == null) {
                    _logger.LogWarning("Product does not exist");
                    response.Success = false;
                    response.Message = ("Product does not exist");
                    return response;
                }

                response.Data = _mapper.Map<List<ProductDto>>(product);
                response.Success = true;
                response.Message = "Products matching price found";
                _logger.LogInformation("Found Product Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }


            return response;
        }


        public async Task<ServiceResponse<List<ProductDto>>> GetPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
        {
            _logger.LogInformation("Attempting to fetch product by price range");
            var response = new ServiceResponse<List<ProductDto>>();

            try
            {
                if (minPrice < 0)
                {
                    _logger.LogWarning("Price not found");
                    response.Success = false;
                    response.Message = "Price not found";
                    return response;
                }

                if (maxPrice < 0)
                {
                    _logger.LogWarning("Price not found");
                    response.Success = false;
                    response.Message = "Price not found";
                    return response;
                }

                var product = await _repo.GetPriceRangeAsync(minPrice, maxPrice);

                if (product == null)
                {
                    _logger.LogWarning("Product does not exist");
                    response.Success = false;
                    response.Message = ("Product does not exist");
                    return response;
                }

                response.Data = _mapper.Map<List<ProductDto>>(product);
                response.Success = true;
                response.Message = "Product price range found";
                _logger.LogInformation("Products found Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProductByNameAsync(string name)
        {
            _logger.LogInformation("Attempting to fetch product by name");
            var response = new ServiceResponse<List<ProductDto>>();

            try
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("Name not found");
                    response.Success = false;
                    response.Message = "Name not found";
                    return response;
                }

                var product = await _repo.GetProductByNameAsync(name);

                response.Data = _mapper.Map<List<ProductDto>>(product);
                response.Success = true;
                response.Message = "Products matching name found";
                _logger.LogInformation("Product name found Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal Server Error");
                response.Success = false;
                response.Message = "Server Error";
            }

            return response;
        }

        public async Task<ServiceResponse<List<ProductDto>>> GetProductByCategoryAsync(string category)
        {
            _logger.LogInformation("Attempting to fetch product by category");
            var response = new ServiceResponse<List<ProductDto>>();

            try
            {
                if (category == null)
                {
                    _logger.LogWarning("category not found");
                    response.Success = false;
                    response.Message = "Ncategoryame not found";
                    return response;
                }

                var product = await _repo.GetProductByCategoryAsync(category);

                response.Data = _mapper.Map<List<ProductDto>>(product);
                response.Success = true;
                response.Message = "Products matching category found";
                _logger.LogInformation("Product category found Successfully");
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
