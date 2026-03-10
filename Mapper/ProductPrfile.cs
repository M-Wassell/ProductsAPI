using AutoMapper;
using ProductsAPI.Classess;
using ProductsAPI.Dto;

namespace ProductsAPI.Mapper
{
    public class ProductPrfile : Profile
    {
        public ProductPrfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
