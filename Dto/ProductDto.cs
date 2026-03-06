using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Dto
{
    public class ProductDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
