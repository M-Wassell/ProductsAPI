using ProductsAPI.Classess;
using ProductsAPI.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAll();
        Task<ServiceResponse<ProductDto>> GetById(int id);
        Task<ServiceResponse<ProductDto>> Create(CreateProductDto product);
        Task<ServiceResponse<ProductDto>> Update(int id, UpdateProductDto upDatingProduct);
        Task<ServiceResponse<bool>> Delete(int id);
    }
}
