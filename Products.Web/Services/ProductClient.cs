using Contracts.Dto;
using Products.Web.Response;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Products.Web.Services
{
    public class ProductClient : IProductAPIClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Good practice
                Converters = { new JsonStringEnumConverter() }
            };
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProductDto>>>("api/Products", options);
            
            //returns a new list instead of a possible null
            return response?.Data ?? new List<ProductDto>();
        }


    }
}
