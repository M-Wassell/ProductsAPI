namespace ProductsAPI.Dto.Query
{
    public record PriceRangeQuery
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
