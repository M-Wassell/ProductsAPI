using Contracts.Dto;
namespace Products.Web.Services
{
    public class ProductService : IProductAPIClient
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {

            var products = await _httpClient.GetFromJsonAsync<List<ProductDto>>("products");
            
            //returns a new list instead of a possible null
            return products ?? new List<ProductDto>();
        }


    }
}
