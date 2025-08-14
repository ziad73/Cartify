
using AutoMapper;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Products.ProductReview;
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
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<CreateProductReview, ProductReview>().ConstructUsing(src => new ProductReview(src.ProductId, src.UserId, src.ReviewerName, src.Comment, src.Rating));
            CreateMap<ProductReview, ProductReviewDTO>().ReverseMap();
        }
    }
}
