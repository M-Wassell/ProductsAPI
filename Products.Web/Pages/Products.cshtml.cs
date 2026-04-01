using Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Products.Web.Services;

namespace Products.Web.Pages
{

    public class ProductsModel : PageModel
    {
        private readonly IProductAPIServiceClient _productClient;

        public ProductsModel(IProductAPIServiceClient productClient)
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
