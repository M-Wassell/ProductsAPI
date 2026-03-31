using ProductsAPI.Classess;
using Contracts.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAll(int pageNumber, int pageSize);
        Task<ServiceResponse<ProductDto>> GetById(int id);
        Task<ServiceResponse<ProductDto>> Create(CreateProductDto product);
        Task<ServiceResponse<ProductDto>> Update(int id, UpdateProductDto upDatingProduct);
        Task<ServiceResponse<bool>> Delete(int id);
        Task<ServiceResponse<List<ProductDto>>> GetExactPriceAsync(decimal price);
        Task<ServiceResponse<List<ProductDto>>> GetPriceRangeAsync(decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
        Task<ServiceResponse<List<ProductDto>>> GetProductByNameAsync(string name);
        Task<ServiceResponse<List<ProductDto>>> GetProductByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<ServiceResponse<List<ProductDto>>> GetProductsByCreatedDate(string createdDate, int pageNumber, int pageSize);

    }
}
