using Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Products.Web.Services;

namespace Products.Web.Pages
{

    public class ProductsModel : PageModel
    {
        private readonly IProductAPIClient _productClient;

        public ProductsModel(IProductAPIClient productClient)
        {
            _productClient = productClient;
        }

        public List<ProductDto> Products { get; set; } = new();
        public async Task OnGetAsync()
        {
           Products = await _productClient.GetAllProductsAsync();
        }
    }
}
