using Microsoft.EntityFrameworkCore;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using SQLitePCL;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductsAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Product>> GetExactPriceAsync(decimal price) { 
        
            return await _context.Products
                .Where(p => p.IsActive && p.Price == price)
                .ToListAsync();
        }

        public async Task<List<Product>> GetPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products
                .Where(p => p.IsActive);
                
            if (minPrice.HasValue) {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }
            return await query.ToListAsync();

        }

        public async Task<List<Product>> GetProductByNameAsync(string name)
        {
            return await _context.Products
                .Where(p => p.IsActive && p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.IsActive && p.Category.ToLower() == category.ToLower())
                .ToListAsync();
        }
    }
}
