
using AutoMapper;
using CartifyBLL.ViewModels.Cart;
using CartifyBLL.ViewModels.Orders;
using CartifyBLL.ViewModels.Product;
using CartifyBLL.ViewModels.Search;
using CartifyBLL.ViewModels.Wishlist;
using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.order;
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
            
            
            
            CreateMap<Order, ManageOrdersVm>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Guest"))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : "N/A"))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.OrderStatus == "Cancelled" ? "Refunded" : src.OrderStatus == "Pending" ? "Pending" : "Paid"));

            CreateMap<Order, OrderDetailsVm>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Guest"))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : "N/A"))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) 
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.OrderStatus == "Cancelled" ? "Refunded" : src.OrderStatus == "Pending" ? "Pending" : "Paid"));

            CreateMap<OrderItem, OrderItemVm>()
                .ForMember(dest => dest.ItemTotal, opt => opt.MapFrom(src => src.Quantity * (src.Price - src.Discount)));
        }
    }
}
