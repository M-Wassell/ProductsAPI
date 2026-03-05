using ProductsAPI.Classess;
using ProductsAPI.Models;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetAll();
        Task<ServiceResponse<Product>>? GetProductById(int id);
        Task<ServiceResponse<Product>> AddProduct(Product product);
        Task<ServiceResponse<Product>>? UpdateProduct(int id, Product upDatingProduct);
        Task<ServiceResponse<Product>>? DeleteProduct(int id);
    }
}
