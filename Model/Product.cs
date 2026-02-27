using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Classess
{
    public class Product
    {
        
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; } = "";
        public decimal Price { get; set; }

        // Add Buy price and Sell Price to allow for costing calculations
    }
}
