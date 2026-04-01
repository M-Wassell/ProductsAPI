using Contracts.Dto;
using Products.Web.Response;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Products.Web.Services
{
    public class ProductServiceClient : IProductAPIServiceClient
    {
        private readonly HttpClient _httpClient;

        public ProductServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public JsonSerializerOptions GetJSonOptions() { 
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProductDto>>>("api/Products", GetJSonOptions());
            
            return response?.Data ?? new List<ProductDto>();
        }

        public async Task<ProductDto> GetProductsById(int Id)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ProductDto>>($"api/Products/{Id}", GetJSonOptions());

            return response?.Data;
        }

    }
}
