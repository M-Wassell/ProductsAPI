using ProductsAPI.Classess;

namespace ProductsAPI.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> SaveChangesAsync();
        Task<List<Product>> GetExactPriceAsync(decimal price);
        Task<List<Product>> GetPriceRangeAsync(decimal? minPrice, decimal? maxPrice);
        Task<List<Product>> GetProductByNameAsync(string name);
        Task<List<Product>> GetProductByCategoryAsync(string category);
    }
}
