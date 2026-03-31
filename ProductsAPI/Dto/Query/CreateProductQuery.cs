using Contracts.Dto;

namespace ProductsAPI.Dto.Query
{
    public record CreateProductQuery
    {
        public CreateProductDto? Dto { get; set; }
    }
}
