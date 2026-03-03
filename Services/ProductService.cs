using ProductsAPI.Classess;

namespace ProductsAPI.Services
{
    public class ProductService : IProductService
    {

        private static readonly List<Product> Products = new List<Product>()
        {
            new Product { Id = 1, Name = "Cup", Price = 3.99m },
            new Product { Id = 2, Name = "Toaster", Price = 4.99m },
            new Product { Id = 3, Name = "Bottle", Price = 3.99m },
            new Product { Id = 4, Name = "TV", Price = 55.99m },
            new Product { Id = 5, Name = "Blanket", Price = 10.99m }
        };

        public List<Product> GetAll()
        {
            return Products;
        }
        public Product? GetProductById(int id)
        {
            return Products.FirstOrDefault(x => x.Id == id);


        }

        public Product AddProduct(Product product)
        {
            var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;// only used for hard coded list items to simulate incrementing the Id
            product.Id = newId;

            Products.Add(product);

            return product;
            
        }
        
        public Product? UpdateProduct(int id, Product upDatingProduct)
        {
            if (id <= 0) {
                return null;
            }
            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null) {
                return null;
            }

            existingProduct.Name = upDatingProduct.Name;
            existingProduct.Price = upDatingProduct.Price;


            return existingProduct;
        }

        public Product? DeleteProduct(int id)
        {
            if (id <= 0) {
                return null;
            }

            var existingProduct = Products.FirstOrDefault(prod => prod.Id == id);

            if (existingProduct == null)
            {
                return null;

            }

            Products.Remove(existingProduct);

            return existingProduct;
        }

    }
}
