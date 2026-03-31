using Contracts.Dto;
namespace ProductsAPI.Dto.Query
{
    public record UpdateProductQuery
    {
        public int Id { get; set; }
        public UpdateProductDto? Dto { get; set; }
    }
}
