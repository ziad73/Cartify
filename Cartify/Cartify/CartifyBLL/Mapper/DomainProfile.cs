
using AutoMapper;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.product;

namespace CartifyBLL.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<CreateProduct, ProductDTO>().ReverseMap();
            CreateMap<CreateProduct, Product>().ReverseMap().ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)); ;
            CreateMap<Product, ProductDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl)); ;
        }
    }
}
