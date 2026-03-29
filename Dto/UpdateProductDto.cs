using ProductsAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Dto
{
    public class UpdateProductDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; }

    }
}
