using ProductsAPI.Classess;

namespace ProductsAPI.Services
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product? GetProductById(int id);
        Product AddProduct(Product product);
        Product? UpdateProduct(int id, Product upDatingProduct);
        Product? DeleteProduct(int id);
    }
}
