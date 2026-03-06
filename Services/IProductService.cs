using ProductsAPI.Classess;
using ProductsAPI.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDto>>> GetAll();
        Task<ServiceResponse<ProductDto>>? GetProductById(int id);
        Task<ServiceResponse<ProductDto>> AddProduct(ProductDto product);
        Task<ServiceResponse<ProductDto>>? UpdateProduct(int id, ProductDto upDatingProduct);
        Task<ServiceResponse<ProductDto>>? DeleteProduct(int id);
    }
}
