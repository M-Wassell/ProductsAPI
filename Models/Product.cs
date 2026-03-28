using ProductsAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Classess
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal BuyPrice { get; set; }
        public int StockQuantity { get; set; }
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
