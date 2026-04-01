using Contracts.Dto;

namespace Products.Web.Services
{
    public interface IProductAPIServiceClient
    {
        Task<List<ProductDto>> GetAllProductsAsync();

        Task<ProductDto> GetProductsById(int Id);
    }
}
