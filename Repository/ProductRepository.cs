using Microsoft.EntityFrameworkCore;
using ProductsAPI.Classess;
using ProductsAPI.Data;
using SQLitePCL;
using System.Threading.Tasks;

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


    }
}
