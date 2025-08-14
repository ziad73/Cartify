
using AutoMapper;
using Cartify.ViewModels;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.order;
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
            CreateMap<Order, ManageOrdersViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Guest"))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : "N/A"))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.OrderStatus == "Cancelled" ? "Refunded" : src.OrderStatus == "Pending" ? "Pending" : "Paid"));

            CreateMap<Order, OrderDetailsViewModel>()
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : "Guest"))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : "N/A"))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()) 
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.OrderStatus == "Cancelled" ? "Refunded" : src.OrderStatus == "Pending" ? "Pending" : "Paid"));

            CreateMap<OrderItem, OrderItemViewModel>()
                .ForMember(dest => dest.ItemTotal, opt => opt.MapFrom(src => src.Quantity * (src.Price - src.Discount)));
        }
    }
}
