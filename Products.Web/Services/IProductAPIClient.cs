using Contracts.Dto;

namespace Products.Web.Services
{
    public interface IProductAPIClient
    {
        Task<List<ProductDto>> GetAllProductsAsync();
    }
}
