using AutoMapper;
using ProductsAPI.Classess;
using ProductsAPI.Dto;

namespace ProductsAPI.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
