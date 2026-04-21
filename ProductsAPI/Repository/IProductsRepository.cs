using ProductsAPI.Classess;
using ProductsAPI.Dto;
using ProductsAPI.Models;

namespace ProductsAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(int pageNumber, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        void Update(Product product);
        //void Delete(Product product);
        Task<bool> SaveChangesAsync();
        Task<List<Product>> GetExactPriceAsync(decimal price);
        Task<List<Product>> GetPriceRangeAsync(decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
        Task<List<Product>> GetProductByNameAsync(string name);
        Task<List<Product>> GetProductByCategoryAsync(string category, int pageNumber, int pageSize);
        Task<List<Product>> GetProductsByCreatedDate(DateTime createdDate, int pageNumber, int pageSize);

    }
}
