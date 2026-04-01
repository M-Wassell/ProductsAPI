using Contracts.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Products.Web.Services;

namespace Products.Web.Pages.Shared
{
    public class DetailsModel : PageModel
    {
        private readonly IProductAPIServiceClient _productClient;

        public DetailsModel(IProductAPIServiceClient productClient)
        {
            _productClient = productClient;
        }



        public ProductDto Product { get; set; }
        public async Task OnGetAsync(int id)
        {
            Product = await _productClient.GetProductsById(id);
        }

    }
}
