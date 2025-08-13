
using AutoMapper;
using CartifyBLL.ViewModels.Cart;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Search;
using CartifyBLL.ViewModels.Wishlist;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.product;
using CartifyDAL.Entities.Search;
using CartifyDAL.Entities.Wishlist;

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
            // New search Mapping
            CreateMap<SearchRequestDTO, SearchCriteria>();
            CreateMap<SearchResult, SearchResultDTO>();
            
            // New Cart mappings
            CreateMap<CartItem, CartItemVm>()
                .ForMember(dest => dest.CartItemId, opt => opt.MapFrom(src => src.Cartitem))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Product.StockQuantity));

           
            // Map WishlistItem -> WishlistItemVm
            CreateMap<WishlistItem, WishlistItemVm>()
                .ForMember(dest => dest.WishListId,      opt => opt.MapFrom(src => src.WishlistId))
                .ForMember(dest => dest.ProductId,       opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName,     opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.ProductPrice,    opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Category,        opt => opt.MapFrom(src => src.Product.Category.Name))
                .ForMember(dest => dest.StockQuantity,   opt => opt.MapFrom(src => src.Product.StockQuantity))
                .ForMember(dest => dest.IsAvailable,     opt => opt.MapFrom(src => src.Product.StockQuantity > 0))
                .ForMember(dest => dest.AddedOn,         opt => opt.MapFrom(src => src.CreatedOn));
        }
    }
}
