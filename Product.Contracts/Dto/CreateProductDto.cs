using Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Contracts.Dto
{
    public class CreateProductDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public ProductCategory Category { get; set; }
        public int? StockQuantity { get; set; }
    }
}
